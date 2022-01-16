using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code
{
    public static class MouseCursor
    {
        public static Vector2 Position;
        public static Vector2 Size;
        public static Rectangle rectangle;
        public static MouseState LastMouseState;
        public static MouseState CurrentMouseState;
        public static void Update()
        {
            LastMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
            Position = CurrentMouseState.Position.ToVector2() + Game1.mainCamera.Position - Game1.WindowSize/2;
            rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
    }
}
