using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace SlimeFight.Screens
{
    public interface IScreen
    {
        void LoadContent(Game1 game);
        void Update(Game1 game, GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
    internal class MainMenu : IScreen
    {
        SpriteFont font;
        string[] menuItems = { "Start", "Exit" };
        int selectedIndex = -1;

        Rectangle[] buttonAreas;
        Texture2D rectTexture;

        public void LoadContent(Game1 game)
        {
            font = game.Content.Load<SpriteFont>("MenuFont");

            rectTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.DarkSlateGray });

            buttonAreas = new Rectangle[menuItems.Length];

            int buttonWidth = 400;
            int buttonHeight = 80;

            int screenWidth = game.GraphicsDevice.Viewport.Width;
            int screenHeight = game.GraphicsDevice.Viewport.Height;

            int totalHeight = menuItems.Length * buttonHeight + (menuItems.Length - 1) * 30;

            int startX = (screenWidth - buttonWidth) / 2;
            int startY = (screenHeight - totalHeight) / 2;

            for (int i = 0; i < menuItems.Length; i++)
            {
                buttonAreas[i] = new Rectangle(startX, startY + i * (buttonHeight + 30), buttonWidth, buttonHeight);
            }
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            TouchCollection touches = TouchPanel.GetState();
            selectedIndex = -1;

            foreach (var touch in touches)
            {
                if (touch.State == TouchLocationState.Pressed)
                {
                    for (int i = 0; i < buttonAreas.Length; i++)
                    {
                        if (buttonAreas[i].Contains(touch.Position))
                        {
                            selectedIndex = i;
                            break;
                        }
                    }
                }
            }

            if (selectedIndex != -1)
            {
                switch (selectedIndex)
                {
                    case 0:
                        game.ChangeScreen(new GameScreen());
                        break;
                    case 1:
                        game.Exit();
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menuItems.Length; i++)
            {
                Color color = i == selectedIndex ? Color.Yellow : Color.White;
                spriteBatch.Draw(rectTexture, buttonAreas[i], Color.White * 0.7f);
                spriteBatch.DrawString(font, menuItems[i], new Vector2(buttonAreas[i].X + 20, buttonAreas[i].Y + 10), color);
            }
        }
    }
}
