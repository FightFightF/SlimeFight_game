using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using SlimeFight.Content.UI;
using SlimeFight.Enemies;
using SlimeFight.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SQLite;
using SlimeFight.DataBase;

namespace SlimeFight.Screens
{
    internal class GameScreen : IScreen
    {
        private GraphicsDevice graphicsDevice;

        private Camera2D camera;
        private List<Slime> slimes;
        private Player_1 player;
        private Joystick joystick;
        private AttackButton attackButton;
        private Texture2D fieldBackground;

        private int score = 0;
        private SpriteFont font;

        private ScoreDatabase scoreDb;

        public void LoadContent(Game1 game)
        {
            graphicsDevice = game.GraphicsDevice;

            joystick = new Joystick(new Vector2(250, game.GraphicsDevice.Viewport.Height - 250), game.GraphicsDevice);
            attackButton = new AttackButton(new Vector2(game.GraphicsDevice.Viewport.Width - 250, game.GraphicsDevice.Viewport.Height - 250), game.GraphicsDevice);

            player = new Player_1(game.GraphicsDevice);
            player.LoadContent(game.Content);

            camera = new Camera2D(game.GraphicsDevice.Viewport);

            fieldBackground = game.Content.Load<Texture2D>("Background/grass");

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string dbPath = Path.Combine(folderPath, "score.db3");

            scoreDb = new ScoreDatabase();

            score = scoreDb.LoadScore();

            font = game.Content.Load<SpriteFont>("BaseFont");

            Random rand = new Random();
            slimes = new List<Slime>();

            for (int i = 0; i < 5; i++)
            {
                Vector2 randomPos = new Vector2(rand.Next(100, 700), rand.Next(100, 400));
                var slime = new Slime(randomPos);
                slime.LoadContent(game.Content);
                slimes.Add(slime);
            }

        }

        private void DrawFieldBackground(SpriteBatch spriteBatch)
        {
            int tileSize = fieldBackground.Width;
            int tilesX = graphicsDevice.Viewport.Width / tileSize + 3;
            int tilesY = graphicsDevice.Viewport.Height / tileSize + 3;

            for (int y = 0; y < tilesY; y++)
            {
                for (int x = 0; x < tilesX; x++)
                {
                    Vector2 tilePosition = new Vector2(
                        x * tileSize - (player.WorldPosition.X % tileSize),
                        y * tileSize - (player.WorldPosition.Y % tileSize)
                    );

                    spriteBatch.Draw(fieldBackground, tilePosition, Color.White);
                }
            }
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            TouchCollection touches = TouchPanel.GetState();

            joystick.Update(touches);
            attackButton.Update(touches);

            Vector2 moveDirection = joystick.GetDirection();
            bool isAttacking = attackButton.IsPressed();

            player.Update(gameTime, moveDirection, isAttacking);

            foreach (var slime in slimes)
            {
                slime.Update(gameTime, player.WorldPosition);
            }

            camera.Follow(player.WorldPosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawFieldBackground(spriteBatch);

            foreach (var slime in slimes)
                slime.Draw(spriteBatch);

            player.Draw(spriteBatch);

            joystick.Draw(spriteBatch);
            attackButton.Draw(spriteBatch);

            spriteBatch.DrawString(font, $"Score: {score}", new Vector2(20, 20), Color.Yellow);
        }
    }
}
