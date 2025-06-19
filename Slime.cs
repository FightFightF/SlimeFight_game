using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace SlimeFight.Enemies
{
    internal class Slime
    {
        private Texture2D slimeSheet;
        private int framesPerRow = 3;
        private int totalRows = 3;
        private int currentFrame = 0;
        private int currentRow = 0;
        private float frameTime = 0.1f;
        private float timer = 0f;
        private int frameWidth;
        private int frameHeight;

        public Vector2 Position;
        private float speed = 100f;

        private Vector2 direction = Vector2.Zero;
        private float changeDirectionInterval = 2f; 
        private float timeSinceDirectionChange = 0f;
        private Random random = new Random();

        private float minX = 0;
        private float maxX = 1920;
        private float minY = 0;
        private float maxY = 1080;

        public Slime(Vector2 startPosition)
        {
            Position = startPosition;
        }

        public void LoadContent(ContentManager content)
        {
            slimeSheet = content.Load<Texture2D>("Slimes/slime_monster_spritesheet");
            frameWidth = slimeSheet.Width / framesPerRow;
            frameHeight = slimeSheet.Height / totalRows;
        }

        public void SetAnimationRow(int row)
        {
            if (currentRow != row)
            {
                currentRow = row;
                currentFrame = 0;
                timer = 0f;
            }
        }

        public void Update(GameTime gameTime, Vector2 playerPosition) 
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeSinceDirectionChange += elapsed;

            if (timeSinceDirectionChange >= changeDirectionInterval || direction == Vector2.Zero)
            {
                float angle = (float)(random.NextDouble() * Math.PI * 2);
                direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                direction.Normalize();

                timeSinceDirectionChange = 0f;
            }

            Position += direction * speed * elapsed;

            bool changed = false;

            if (Position.X < minX)
            {
                Position.X = minX;
                changed = true;
            }
            else if (Position.X > maxX)
            {
                Position.X = maxX;
                changed = true;
            }

            if (Position.Y < minY)
            {
                Position.Y = minY;
                changed = true;
            }
            else if (Position.Y > maxY)
            {
                Position.Y = maxY;
                changed = true;
            }

            if (changed)
            {
                float angle = (float)(random.NextDouble() * Math.PI * 2);
                direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                direction.Normalize();
                timeSinceDirectionChange = 0f;
            }

            SetAnimationRow(1);

            timer += elapsed;
            if (timer >= frameTime)
            {
                timer = 0f;
                currentFrame = (currentFrame + 1) % framesPerRow;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, currentRow * frameHeight, frameWidth, frameHeight);
            float scale = 6f;
            spriteBatch.Draw(slimeSheet, Position, sourceRect, Color.White, 0f, new Vector2(frameWidth / 2f, frameHeight / 2f), scale, SpriteEffects.None, 0f);
        }
    }
}
