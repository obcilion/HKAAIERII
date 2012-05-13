using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

// Code: Kenneth

namespace HKAAIERII
{
    class PauseMenu
    {
        private List<string> MenuItems;
        public string Title { get; set; }
        Texture2D MenuBackground;
        Color PauseMenuColor = new Color(50, 50, 50, 50);


        public PauseMenu(Texture2D menuBackground)
        {
            MenuBackground = menuBackground;
            Title = "Pause Menu";
            MenuItems = new List<string>();
            MenuItems.Add("Continue Game");
            MenuItems.Add("Main Menu");
            Game1.Selected = 0;
        }

        public void Update()
        {
            if (InputHandler.Instance.IsDownPressed())
            {
                Game1.Selected++;
                if (Game1.Selected > MenuItems.Count - 1) Game1.Selected = 0;
            }
            if (InputHandler.Instance.IsUpPressed())
            {
                Game1.Selected--;
                if (Game1.Selected < 0) Game1.Selected = MenuItems.Count - 1;
            }

            if (InputHandler.Instance.IsActionPressed())
            {
                Game1.HasSelected = true;
            }

            if (Game1.HasSelected)
            {
                switch (Game1.Selected)
                {
                    case 0:
                        Game1.ResetMenu();
                        Game1.IsPaused = false;
                        break;
                    case 1:
                        Game1.ResetMenu();
                        Game1.IsPaused = false;
                        Game1.gamestate = Game1.GameStates.Menu;
                        break;
                }
            }
        }
        public void Draw(SpriteBatch batch, int screenWidth, SpriteFont MenuTitleFont, SpriteFont MenuFont)
        {
            batch.Draw(MenuBackground, Vector2.Zero, PauseMenuColor);
            batch.DrawString(MenuTitleFont, Title, new Vector2(screenWidth / 2 - MenuTitleFont.MeasureString(Title).X / 2, 50), Color.Black);
            int yPos = 250;
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Color colour = Color.Black;
                if (i == Game1.Selected)
                {
                    colour = Color.White;
                }
                batch.DrawString(MenuFont, MenuItems[i], new Vector2(screenWidth / 2 - MenuFont.MeasureString(MenuItems[i]).X / 2, yPos), colour);
                yPos += MenuFont.LineSpacing;
            }
        }
    }
}

