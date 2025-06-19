using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace SlimeFight.Content.UI
{
    public class AttackButton
    {
        private Vector2 position;
        private float radius;
        private bool pressed = false;
        private int id = -1;

        private Texture2D pixel;

        public AttackButton(Vector2 position, GraphicsDevice graphicsDevice, float radius = 90f)
        {
            this.position = position;
            this.radius = radius;

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Update(TouchCollection touches)
        {
            pressed = false;
            foreach (var touch in touches)
            {
                if (touch.State == TouchLocationState.Pressed && id == -1)
                {
                    if (Vector2.Distance(touch.Position, position) < radius)
                    {
                        id = touch.Id;
                        pressed = true;
                    }
                }
                else if (touch.Id == id)
                {
                    if (touch.State == TouchLocationState.Moved || touch.State == TouchLocationState.Pressed)
                    {
                        pressed = true;
                    }
                    else
                    {
                        id = -1;
                        pressed = false;
                    }
                }
            }
        }

        public bool IsPressed() => pressed;

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawCircle(spriteBatch, position, radius, pressed ? Color.Red : Color.DarkGray);
        }

        private void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, Color color)
        {
            int segments = 30;
            float increment = MathHelper.TwoPi / segments;
            Vector2[] vertices = new Vector2[segments];

            for (int i = 0; i < segments; i++)
            {
                float angle = i * increment;
                vertices[i] = new Vector2(
                    center.X + radius * (float)Math.Cos(angle),
                    center.Y + radius * (float)Math.Sin(angle));
            }

            for (int i = 0; i < segments; i++)
            {
                Vector2 start = vertices[i];
                Vector2 end = vertices[(i + 1) % segments];
                DrawLine(spriteBatch, start, end, color, 4);
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int thickness)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            float length = edge.Length();

            spriteBatch.Draw(pixel, new Rectangle((int)start.X, (int)start.Y, (int)length, thickness), null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
