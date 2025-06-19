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
using SlimeFight.Content.UI;
using static Android.Icu.Text.Transliterator;

namespace SlimeFight.Player
{
    internal class Player_1
    {
        private GraphicsDevice graphicsDevice;

        public Vector2 WorldPosition = new Vector2(0, 0);

        private Texture2D idleSheet;
        private Texture2D runSheet;
        private Texture2D currentSheet;
        private Texture2D attackSheet;

        private int idleFrameCount = 4;
        private int runFrameCount = 6;
        private int currentFrameCount;
        private int attackFrameCount = 4;

        private bool attackInProgress = false;
        private float attackDuration = 0.4f;
        private float attackElapsed = 0f;

        private int currentFrame = 0;
        private float frameTime = 0.1f;
        private float timer = 0f;

        private int frameWidth;
        private int frameHeight;
        private bool facingRight = true;

        public int MaxHealth { get; private set; } = 100;
        public int CurrentHealth { get; private set; } = 100;

        private Texture2D pixel;
        private Texture2D fieldBackground;

        public Player_1(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void LoadContent(ContentManager content)
        {
            idleSheet = content.Load<Texture2D>("Player1/Pink_Monster_Idle_4");
            runSheet = content.Load<Texture2D>("Player1/Pink_Monster_Run_6");
            attackSheet = content.Load<Texture2D>("Player1/Pink_Monster_Attack1_4");

            currentSheet = idleSheet;
            currentFrameCount = idleFrameCount;

            frameWidth = currentSheet.Width / currentFrameCount;
            frameHeight = currentSheet.Height;

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            fieldBackground = content.Load<Texture2D>("Background/grass");
        }
        public void Update(GameTime gameTime, Vector2 moveDirection, bool isAttacking)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += elapsed;

            if (attackInProgress)
            {
                attackElapsed += elapsed;

                if (timer >= frameTime)
                {
                    timer = 0f;
                    currentFrame = (currentFrame + 1) % attackFrameCount;
                }

                if (attackElapsed >= attackDuration)
                {
                    attackInProgress = false;

                    currentSheet = idleSheet;
                    currentFrameCount = idleFrameCount;
                    currentFrame = 0;
                    timer = 0f;
                    frameWidth = currentSheet.Width / currentFrameCount;
                    frameHeight = currentSheet.Height;
                }

                return;
            }

            if (isAttacking)
            {
                attackInProgress = true;
                attackElapsed = 0f;

                currentSheet = attackSheet;
                currentFrameCount = attackFrameCount;
                currentFrame = 0;
                timer = 0f;

                frameWidth = currentSheet.Width / currentFrameCount;
                frameHeight = currentSheet.Height;

                return;
            }

            bool isRunning = moveDirection.LengthSquared() > 0.01f;

            if (isRunning)
            {
                WorldPosition += moveDirection * 320f * elapsed;

                if (Math.Abs(moveDirection.X) > 0.01f)
                    facingRight = moveDirection.X > 0;

                if (currentSheet != runSheet)
                {
                    currentSheet = runSheet;
                    currentFrameCount = runFrameCount;
                    currentFrame = 0;
                    timer = 0f;
                    frameWidth = currentSheet.Width / currentFrameCount;
                    frameHeight = currentSheet.Height;
                }
            }
            else
            {
                if (currentSheet != idleSheet)
                {
                    currentSheet = idleSheet;
                    currentFrameCount = idleFrameCount;
                    currentFrame = 0;
                    timer = 0f;
                    frameWidth = currentSheet.Width / currentFrameCount;
                    frameHeight = currentSheet.Height;
                }
            }

            if (timer >= frameTime)
            {
                timer = 0f;
                currentFrame = (currentFrame + 1) % currentFrameCount;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            SpriteEffects flip = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Vector2 screenPosition = new Vector2(
                graphicsDevice.Viewport.Width / 2 - (frameWidth * 4) / 2,
                graphicsDevice.Viewport.Height / 2 - (frameHeight * 4) / 2);

            spriteBatch.Draw(currentSheet, screenPosition, sourceRect, Color.White, 0f, Vector2.Zero, 4f, flip, 0f);

            Rectangle healthBarBackground = new Rectangle(30, 30, 300, 20);
            Rectangle healthBarFill = new Rectangle(30, 30, (int)(300 * ((float)CurrentHealth / MaxHealth)), 20);

            spriteBatch.Draw(pixel, healthBarBackground, Color.DarkRed);
            spriteBatch.Draw(pixel, healthBarFill, Color.Red);
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0) CurrentHealth = 0;
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        }
    }
}
