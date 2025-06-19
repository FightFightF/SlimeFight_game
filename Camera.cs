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

namespace SlimeFight.Screens
{
    public class Camera2D
    {
        public Vector2 Position { get; private set; }
        public float Zoom { get; set; } = 1f;
        public Viewport Viewport { get; private set; }

        public Camera2D(Viewport viewport)
        {
            Viewport = viewport;
        }

        public void Follow(Vector2 targetPosition)
        {
            Position = targetPosition;
        }

        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                Matrix.CreateScale(Zoom) *
                Matrix.CreateTranslation(new Vector3(Viewport.Width * 0.5f, Viewport.Height * 0.5f, 0));
        }
    }
}
