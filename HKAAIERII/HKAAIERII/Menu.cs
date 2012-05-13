using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Code: Hanne

namespace HKAAIERII
{
    class Menu
    {
        private List<string> MenuItems;
        private List<string> CreditsItems = new List<string>();
        public string Title { get; set; }
        Texture2D MenuBackground;

        // Sets the menu options
        public Menu(Texture2D menuBackground)
        {
            MenuBackground = menuBackground;
            Title = "Hello Kitty Action Adventure Island Extreme Reloaded II";
            MenuItems = new List<string>();
            MenuItems.Add("Start Game");
            MenuItems.Add("Credits");
            MenuItems.Add("Exit Game");

            CreditsItems.Add("Tarje Hellebust jr.");
            CreditsItems.Add("Hanne Lohne Try");
            CreditsItems.Add("Kenneth Hagaas");
            CreditsItems.Add("Raymond Gulbrandsen");

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

            // Resets game and menu, switches gamestate or exits game
            if (Game1.HasSelected)
            {
                switch (Game1.Selected)
                {
                    case 0:
                        Game1.ResetGame();
                        Game1.ResetMenu();
                        Game1.gamestate = Game1.GameStates.Running;
                        break;
                    case 1:
                        Game1.ResetMenu();
                        Game1.gamestate = Game1.GameStates.Credits;
                        break;
                    case 2:
                        Game1.ExitGame = true;
                        break;
                }
            }
        }
        public void DrawMenu(SpriteBatch batch, int screenWidth, SpriteFont MenuTitleFont, SpriteFont MenuFont)
        {
            batch.Draw(MenuBackground, Vector2.Zero, Color.White);
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

        public void DrawCredtis(SpriteBatch batch, int screenWidth, SpriteFont MenuTitleFont, SpriteFont MenuFont)
        {
            batch.Draw(MenuBackground, Vector2.Zero, Color.White);

            batch.DrawString(MenuTitleFont, "MADE BY:", new Vector2(screenWidth / 2 - MenuTitleFont.MeasureString("MADE BY:").X / 2, 50), Color.Black);

            int yPos = 250;
            for (int i = 0; i < CreditsItems.Count; i++)
            {
                batch.DrawString(MenuFont, CreditsItems[i], new Vector2(screenWidth / 2 - MenuFont.MeasureString(CreditsItems[i]).X / 2, yPos), Color.Black);
                yPos += MenuFont.LineSpacing;
            }
        }

        public void DrawEndScreen(SpriteBatch batch, int screenWidth, SpriteFont arial)
        {
            batch.Draw(MenuBackground, Vector2.Zero, Color.White);

            batch.DrawString(arial, "Congratulations, you found Mimmy!", new Vector2(screenWidth / 2 - arial.MeasureString("Congratulations, you found Mimmy!").X / 2, 300), Color.Black);
        }
    }
}