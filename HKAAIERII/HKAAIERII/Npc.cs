using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Code: Kenneth

namespace HKAAIERII
{
    public class Npc
    {
        public Boolean IsDialogActive = false;

        // NPC items        
        private Texture2D Texture;
        private Rectangle NpcRectangle;
        private Vector2 Position;
        private Vector2 Offset;

        // Dialog box items
        private Texture2D DialogBoxTexture;
        private Rectangle DialogBox;
        private Color color = new Color(100, 100, 100, 0);

        //Dialog text items
        private SpriteFont DialogFont;
        private String DialogText;
        private Vector2 DialogPosition;


        public Npc(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;

            Offset.X = Texture.Width / 2;
            Offset.Y = Texture.Height;
        }

        public void Update(Player player)
        {
            // Creates a rectangle that surounds the lower part of the NPC
            NpcRectangle = new Rectangle((int)Position.X - Texture.Width, (int)Position.Y - Texture.Height / 4, Texture.Width * 2, Texture.Height);

            // Checks if Player collides with the NPC
            if (NpcRectangle.Contains((int)player.Position.X, (int)player.Position.Y))
            {
                IsDialogActive = true;
            }
            else
                IsDialogActive = false;
        }

        public void Draw(SpriteBatch SpriteBatch)
        {
            // Draws NPC and Dialog
            SpriteBatch.Draw(Texture, Position - Offset, Color.White);

            if (IsDialogActive)
            {
                SpriteBatch.Draw(DialogBoxTexture, DialogBox, color);
                SpriteBatch.DrawString(DialogFont, DialogText, DialogPosition, Color.Pink);
            }
        }

        public void Dialog(Texture2D dialogBoxTexture, SpriteFont dialogFont, String dialogText, Vector2 dialogPosition)
        {
            DialogBoxTexture = dialogBoxTexture;
            DialogFont = dialogFont;
            DialogText = dialogText;
            DialogPosition = dialogPosition;

            // Creates a rectangel with 15px from text to edge
            DialogBox = new Rectangle((int)DialogPosition.X - 15, (int)DialogPosition.Y - 15, (int)DialogFont.MeasureString(DialogText).X + 30, (int)DialogFont.MeasureString(DialogText).Y + 30);
        }
    }
}
