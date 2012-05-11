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
            Credits,
            End
        }

        public static GameStates gamestate;

        //menu items
        Menu menu;
        PauseMenu pauseMenu;
        SpriteFont MenuFont;
        SpriteFont MenuTitleFont;
        Texture2D MenuBackground;
        private int screenWidth;
        private int screenHeight;
        public static Boolean HasSelected;
        public static Boolean ExitGame;
        public static Boolean IsPaused;
        public static int Selected;

        public static void ResetMenu()
        {
            HasSelected = false;
            Selected = 0;
        }

        public static void ResetGame()
        {
            HasEnteredHouseOne = false;
            HasEnteredHouseTwo = false;
            ActiveLevel = Island1;
            ActiveNpc = null;
            Player.Position = new Vector2(640, 500);
            Player.Sprite.AnimationId = 2;
        }

        //the different levels in the game
        static Level Island1;
        Level Island1House;
        Level Island2;
        Level Island2House;
        Level Island3;
        Level Island3House;
        static Level ActiveLevel;

        static Player Player;

        // NPCs
        Npc Mama;
        Npc Papa;
        Npc Mimmy;
        static Npc ActiveNpc;

        //transition between levels
        Rectangle IslandOneBridge = new Rectangle(1225, 340, 220, 100);
        Rectangle IslandOneSouth = new Rectangle(50, 675, 650, 50);
        Rectangle IslandTwoBridge = new Rectangle(10, 285, 30, 100);
        Rectangle IslandThreeNorth = new Rectangle(50, 10, 640, 60);

        Rectangle IslandOneHouse = new Rectangle(120, 488, 55, 10);
        Rectangle IslandTwoHouse = new Rectangle(652, 374, 55, 10);
        Rectangle IslandThreeHouse = new Rectangle(829, 280, 55, 10);

        Rectangle HouseOne = new Rectangle(550, 525, 80, 20);
        Rectangle HouseTwo = new Rectangle(520, 530, 65, 15);
        Rectangle HouseThree = new Rectangle(545, 560, 70, 15);

        // Booleans to check if player has entered the houses
        static Boolean HasEnteredHouseOne;
        static Boolean HasEnteredHouseTwo;

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
            menu = new Menu(MenuBackground);
            pauseMenu = new PauseMenu(MenuBackground);

            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;
        }


        protected override void LoadContent()
        {
            //create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load sprite font
            MenuFont = Content.Load<SpriteFont>("MenuFont");
            MenuTitleFont = Content.Load<SpriteFont>("MenuTitleFont");
            SpriteFont DialogFont = Content.Load<SpriteFont>("DialogFont");

            // Load MenuBackground
            MenuBackground = Content.Load<Texture2D>("MenuBackground");

            //initialize the different levels used
            Island1 = new Level(Content.Load<Texture2D>("bg1"), Content.Load<Texture2D>("bg1_collision"), new Vector2(1200, 370), 1);
            Island1House = new Level(Content.Load<Texture2D>("mamahouse"), Content.Load<Texture2D>("mamahouse_collision"), new Vector2(590, 500), 3);

            Island2 = new Level(Content.Load<Texture2D>("bg2"), Content.Load<Texture2D>("bg2_collision"), new Vector2(50, 330), 2);
            Island2House = new Level(Content.Load<Texture2D>("papahouse"), Content.Load<Texture2D>("papahouse_collision"), new Vector2(550, 500), 3);

            Island3 = new Level(Content.Load<Texture2D>("bg3"), Content.Load<Texture2D>("bg3_collision"), new Vector2(378, 150), 0);
            Island3House = new Level(Content.Load<Texture2D>("mimmihouse"), Content.Load<Texture2D>("mimmihouse_collision"), new Vector2(575, 530), 3);

            ActiveLevel = Island1;


            //load the sprite and player
            Sprite playerSprite = new Sprite(Content.Load<Texture2D>("charsheet"), 58, 73, 1f);

            Player = new Player(playerSprite);

            // Load the NPCs
            Mama = new Npc(Content.Load<Texture2D>("Mama"), new Vector2(590, 225));
            Papa = new Npc(Content.Load<Texture2D>("Papa"), new Vector2(400, 230));
            Mimmy = new Npc(Content.Load<Texture2D>("Mimmy"), new Vector2(420, 260));

            // DialogTexture
            Texture2D DialogBoxTexture = Content.Load<Texture2D>("blank");

            // Give NPCs Dialog text
            Mama.Dialog(DialogBoxTexture, DialogFont, "I dont know where Mimmy is, cross the bridge and ask Papa", new Vector2(570, 65));
            Papa.Dialog(DialogBoxTexture, DialogFont, "Mimmy is in the house south of Mama", new Vector2(380, 70));
            Mimmy.Dialog(DialogBoxTexture, DialogFont, "I am your Mimmy", new Vector2(400, 110));
        }


        protected override void UnloadContent()
        {

        }


        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (ExitGame)
                this.Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            InputHandler.Instance.Update();

            if (gamestate == GameStates.Running)
            {
                // Pauses game if abort key is pressed, runs the pauseMenu update
                if (InputHandler.Instance.IsAbortPressed())
                    IsPaused = true;

                if (IsPaused)
                    pauseMenu.Update();

                else
                {
                    // Runs the Player Update method
                    Player.Update(deltaTime, ActiveLevel.Collision);

                    // Runs NPC update if there is an active NPC
                    if (ActiveNpc != null)
                        ActiveNpc.Update(Player);

                    if (ActiveLevel == Island1)
                    {
                        if (IslandOneBridge.Contains((int)Player.Position.X, (int)Player.Position.Y))
                        {
                            ActiveLevel = Island2;
                            Player.Position = ActiveLevel.StartPosition;
                            Player.Sprite.AnimationId = ActiveLevel.StartAnimation;
                        }
                        if (IslandOneSouth.Contains((int)Player.Position.X, (int)Player.Position.Y))
                        {
                            ActiveLevel = Island3;
                            Player.Position = ActiveLevel.StartPosition;
                            Player.Sprite.AnimationId = ActiveLevel.StartAnimation;
                        }
                        if (IslandOneHouse.Contains((int)Player.Position.X, (int)Player.Position.Y))
                        {
                            ActiveLevel = Island1House;
                            Player.Position = ActiveLevel.StartPosition;
                            Player.Sprite.AnimationId = ActiveLevel.StartAnimation;
                        }
                    }

                    if (ActiveLevel == Island1House)
                    {
                        ActiveNpc = Mama;
                        if (Mama.IsDialogActive)
                            HasEnteredHouseOne = true;

                        if (HouseOne.Contains((int)Player.Position.X, (int)Player.Position.Y))
                        {
                            ActiveLevel = Island1;
                            Player.Position = new Vector2(145, 500);
                            Player.Sprite.AnimationId = 0;
                            ActiveNpc = null;
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
                        if (IslandTwoHouse.Contains((int)Player.Position.X, (int)Player.Position.Y)
                            && HasEnteredHouseOne)
                        {
                            ActiveLevel = Island2House;
                            Player.Position = ActiveLevel.StartPosition;
                            Player.Sprite.AnimationId = ActiveLevel.StartAnimation;
                        }
                    }

                    if (ActiveLevel == Island2House)
                    {
                        ActiveNpc = Papa;
                        if (Papa.IsDialogActive)
                            HasEnteredHouseTwo = true;

                        if (HouseTwo.Contains((int)Player.Position.X, (int)Player.Position.Y))
                        {
                            ActiveLevel = Island2;
                            Player.Position = new Vector2(680, 390);
                            Player.Sprite.AnimationId = 0;
                            ActiveNpc = null;
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
                        if (IslandThreeHouse.Contains((int)Player.Position.X, (int)Player.Position.Y)
                            && HasEnteredHouseOne && HasEnteredHouseTwo)
                        {
                            ActiveLevel = Island3House;
                            Player.Position = ActiveLevel.StartPosition;
                            Player.Sprite.AnimationId = ActiveLevel.StartAnimation;
                        }
                    }

                    if (ActiveLevel == Island3House)
                    {
                        ActiveNpc = Mimmy;
                        if (Mimmy.IsDialogActive)
                            gamestate = GameStates.End;

                        if (HouseThree.Contains((int)Player.Position.X, (int)Player.Position.Y))
                        {
                            ActiveLevel = Island3;
                            Player.Position = new Vector2(850, 300);
                            Player.Sprite.AnimationId = 0;
                            ActiveNpc = null;
                        }
                    }
                }
            }

            else if (gamestate == GameStates.Menu)
            {
                menu.Update();
            }

            else if (gamestate == GameStates.Credits)
            {
                if (InputHandler.Instance.IsActionPressed())
                {
                    gamestate = GameStates.Menu;
                }
            }
            else if (gamestate == GameStates.End)
            {
                if (InputHandler.Instance.IsActionPressed())
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
                // Draws a transparent pauseMenu with the game in the background
                if (IsPaused)
                {
                    ActiveLevel.Draw(spriteBatch);

                    if (ActiveNpc != null)
                        ActiveNpc.Draw(spriteBatch);

                    Player.Draw(spriteBatch);

                    pauseMenu.Draw(spriteBatch, screenWidth, MenuTitleFont, MenuFont);
                }
                else
                {
                    ActiveLevel.Draw(spriteBatch);

                    // Draws the NPC if there is an NPC active
                    if (ActiveNpc != null)
                        ActiveNpc.Draw(spriteBatch);

                    Player.Draw(spriteBatch);
                }
            }
            else if (gamestate == GameStates.Menu)
            {
                menu.DrawMenu(spriteBatch, screenWidth, MenuTitleFont, MenuFont);
            }
            else if (gamestate == GameStates.Credits)
            {
                menu.DrawCredtis(spriteBatch, screenWidth, MenuTitleFont, MenuFont);
            }
            else if (gamestate == GameStates.End)
            {
                menu.DrawEndScreen(spriteBatch, screenWidth, MenuFont);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}