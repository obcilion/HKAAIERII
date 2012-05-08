using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HKAAIERII
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //game state management
        public enum GameStates
        {
            Menu,
            Running,
            End
        }

        public static GameStates gamestate;

        //menu items
        Menu menu;
        SpriteFont arial;
        private int screenWidth;
        private int screenHeight;

        //the different levels in the game
        Level Island1;
        Level Island2;
        Level Island3;
        Level ActiveLevel;

        Player Player;

        //transition between levels
        Rectangle IslandOneBridge = new Rectangle(1225, 340, 220, 50);
        Rectangle IslandOneSouth = new Rectangle(50, 675, 650, 50);
        Rectangle IslandTwoBridge = new Rectangle(10, 285, 30, 50);
        Rectangle IslandThreeNorth = new Rectangle(50, 10, 640, 60);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
        }


        protected override void Initialize()
        {
            base.Initialize();

            gamestate = GameStates.Menu;
            menu = new Menu();

            screenHeight = 600;
            screenWidth = 800;
        }


        protected override void LoadContent()
        {
            //create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load sprite font
            arial = Content.Load<SpriteFont>("Arial");

            //initialize the different levels used
            Island1 = new Level(Content.Load<Texture2D>("bg1"), Content.Load<Texture2D>("bg1_collision"), new Vector2(1200, 370), 1);

            Island2 = new Level(Content.Load<Texture2D>("bg2"), Content.Load<Texture2D>("bg2_collision"), new Vector2(50, 330), 2);

            Island3 = new Level(Content.Load<Texture2D>("bg3"), Content.Load<Texture2D>("bg3_collision"), new Vector2(378, 150), 0);

            ActiveLevel = Island1;


            //load the sprite and player
            Sprite playerSprite = new Sprite(Content.Load<Texture2D>("charsheet"), 43, 53, 1f);

            Player = new Player(playerSprite);
        }


        protected override void UnloadContent()
        {

        }


        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();

            if (gamestate == GameStates.Running)
            {
                //HVA ER DENNE FOR?
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                //HVA GJØR TESTEN?
                Player.Update(deltaTime, ActiveLevel.Collision);

                if (ActiveLevel == Island1)
                {
                    if (IslandOneBridge.Contains((int)Player.Position.X, (int)Player.Position.Y))
                    {
                        ActiveLevel = Island2;
                        Player.Position = ActiveLevel.StartPosition;
                        Player.Sprite.AnimationId = ActiveLevel.StartAnimation;
                    }
                }
                if (ActiveLevel == Island1)
                {
                    if (IslandOneSouth.Contains((int)Player.Position.X, (int)Player.Position.Y))
                    {
                        ActiveLevel = Island3;
                        Player.Position = ActiveLevel.StartPosition;
                        Player.Sprite.AnimationId = ActiveLevel.StartAnimation;
                    }
                }
                if (ActiveLevel == Island2)
                {
                    if (IslandTwoBridge.Contains((int)Player.Position.X, (int)Player.Position.Y))
                    {
                        ActiveLevel = Island1;
                        Player.Position = ActiveLevel.StartPosition;
                        Player.Sprite.AnimationId = ActiveLevel.StartAnimation;
                    }
                }
                if (ActiveLevel == Island3)
                {
                    if (IslandThreeNorth.Contains((int)Player.Position.X, (int)Player.Position.Y))
                    {
                        ActiveLevel = Island1;
                        Player.Position = new Vector2(300, 600);
                        Player.Sprite.AnimationId = ActiveLevel.StartAnimation;
                    }
                }
            }
            else if (gamestate == GameStates.Menu)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    menu.Iterator++;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    menu.Iterator--;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    if (menu.Iterator == 0)
                    {
                        gamestate = GameStates.Running;
                    }
                    else if (menu.Iterator == 2)
                    {
                        this.Exit();
                    }
                    menu.Iterator = 0;
                }
            }
            else if (gamestate == GameStates.End)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    gamestate = GameStates.Menu;
                }
            }
            base.Update(gameTime);
        }


        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (gamestate == GameStates.Running)
            {
                ActiveLevel.Draw(spriteBatch);
                Player.Draw(spriteBatch);
            }
            else if (gamestate == GameStates.Menu)
            {
                menu.DrawMenu(spriteBatch, screenWidth, arial);
            }
            else if (gamestate == GameStates.End)
            {
                menu.DrawEndScreen(spriteBatch, screenWidth, arial);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
