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
    public class Joystick
    {
        private Vector2 position;
        private float radius;
        private Vector2 direction = Vector2.Zero;

        private int id = -1;
        private Texture2D textureBase;
        private Texture2D textureKnob;

        public Joystick(Vector2 position, GraphicsDevice graphicsDevice)
        {
            this.position = position;
            radius = 100f;

            textureBase = new Texture2D(graphicsDevice, 200, 200);
            Color[] baseData = new Color[200 * 200];
            for (int i = 0; i < baseData.Length; i++) baseData[i] = Color.Gray * 0.5f;
            textureBase.SetData(baseData);

            textureKnob = new Texture2D(graphicsDevice, 80, 80);
            Color[] knobData = new Color[80 * 80];
            for (int i = 0; i < knobData.Length; i++) knobData[i] = Color.White;
            textureKnob.SetData(knobData);
        }

        public void Update(TouchCollection touches)
        {
            direction = Vector2.Zero;
            foreach (var touch in touches)
            {
                if (Vector2.Distance(touch.Position, position) < radius)
                {
                    Vector2 diff = touch.Position - position;
                    float length = diff.Length();
                    if (length > radius) length = radius;
                    direction = diff / radius;  
                    direction = Vector2.Clamp(direction, new Vector2(-1, -1), new Vector2(1, 1));
                }
            }
        }

        public Vector2 GetDirection()
        {
            return direction;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureBase, position - new Vector2(textureBase.Width / 2, textureBase.Height / 2), Color.White);

            Vector2 knobPos = position + direction * radius * 0.5f;
            spriteBatch.Draw(textureKnob, knobPos - new Vector2(textureKnob.Width / 2, textureKnob.Height / 2), Color.White);
        }
    }
}
