using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RISE.Code.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.UI
{
    public class BorderThickness
    {
        internal int Top, Right, Bottom, Left;
        internal BorderThickness() { }
        internal BorderThickness(int Top, int Right, int Bottom, int Left)
        {
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
            this.Left = Left;
        }
    }
    public class MouseRectangle
    {
        GraphicsDeviceManager Graphics;

        Texture2D OriginalTexture;

        Texture2D BackgraoundTexture;
        Texture2D TopLineTexture, RightLineTexture, BottomLineTexture, LeftLineTexture;
        Texture2D TopLeftCornerTexture, TopRightCornerTexture, BottomRightCornerTexture, BottomLeftCornerTexture;

        Rectangle BackgraoundRec, BackgraoundSourceRec;

        Rectangle TopLineRec, RightLineRec, BottomLineRec, LeftLineRec,
                  TopLineSourceRec, RightSourceLineRec, BottomSourceLineRec, LeftSourceLineRec;

        Rectangle TopLeftCorenerRec, BottomLeftCorener, TopRightCorener, BottomRightCorener,
                  TopLeftCorenerSourceRec, BottomLeftCorenerSourceRec, TopRightCorenerSourceRec, BottomRightCorenerSourceRec;

        BorderThickness borderThickness;
        Vector2 Position, Size, CornerSize;
        public Rectangle rectangle;

        bool ChangeRectanglePosition = true;
        Vector2 LastPositionPressed;

        float Alpha = 0.3f;

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        public void LoadContent()
        {
            OriginalTexture = Game1.textures[TextureNames.MOUSE_RECTANGLE];
        }

        public void UpdateOnce()
        {
            borderThickness = new BorderThickness(1, 1, 1, 1);
            CornerSize = new Vector2(2, 2);

            SetCropRectangle();
            CropTextures(Graphics);
        }

        public void Update(GameTime gameTime)
        {
            if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed)
            {
                if (ChangeRectanglePosition)
                {
                    Position = MouseCursor.Position;
                    LastPositionPressed = MouseCursor.Position;
                    ChangeRectanglePosition = false;
                }
            }

            if (MouseCursor.Position.X > LastPositionPressed.X && MouseCursor.Position.Y > LastPositionPressed.Y)
            {
                if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    Position.X = LastPositionPressed.X;
                    Size = new Vector2(MouseCursor.Position.X - LastPositionPressed.X, MouseCursor.Position.Y - LastPositionPressed.Y);
                }
            }

            if (MouseCursor.Position.X < LastPositionPressed.X && MouseCursor.Position.Y > LastPositionPressed.Y)
            {
                if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    Position.X = MouseCursor.Position.X;
                    Size = new Vector2(LastPositionPressed.X - MouseCursor.Position.X, MouseCursor.Position.Y - LastPositionPressed.Y);
                }
            }

            if (MouseCursor.Position.X > LastPositionPressed.X && MouseCursor.Position.Y < LastPositionPressed.Y)
            {
                if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    Position.Y = MouseCursor.Position.Y;
                    Size = new Vector2(MouseCursor.Position.X - LastPositionPressed.X, LastPositionPressed.Y - MouseCursor.Position.Y);
                }
            }

            if (MouseCursor.Position.X < LastPositionPressed.X && MouseCursor.Position.Y < LastPositionPressed.Y)
            {
                if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    Position = new Vector2(MouseCursor.Position.X, MouseCursor.Position.Y);
                    Size = new Vector2(LastPositionPressed.X - MouseCursor.Position.X, LastPositionPressed.Y - MouseCursor.Position.Y);
                }
            }

            if (MouseCursor.LastMouseState.LeftButton == ButtonState.Released)
            {
                ChangeRectanglePosition = true;
                Position = Vector2.Zero;
                Size = Vector2.Zero;
            }

            rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            MoveAndResizeUI();
        }

        void SetCropRectangle()
        {
            BackgraoundSourceRec = new Rectangle((int)borderThickness.Left, (int)borderThickness.Top, (int)OriginalTexture.Width - (borderThickness.Right * 2), (int)(OriginalTexture.Height - (borderThickness.Bottom * 2)));

            TopLineSourceRec = new Rectangle((int)CornerSize.X, 0, (int)(OriginalTexture.Width - CornerSize.X * 2), borderThickness.Top);
            RightSourceLineRec = new Rectangle((int)(OriginalTexture.Width - borderThickness.Right), (int)CornerSize.Y, borderThickness.Right, (int)(OriginalTexture.Height - CornerSize.Y * 2));
            BottomSourceLineRec = new Rectangle((int)CornerSize.X, (int)(OriginalTexture.Height - borderThickness.Bottom), (int)(OriginalTexture.Width - CornerSize.X * 2), borderThickness.Bottom);
            LeftSourceLineRec = new Rectangle(0, (int)CornerSize.Y, borderThickness.Left, (int)(OriginalTexture.Height - CornerSize.Y * 2));

            TopLeftCorenerSourceRec = new Rectangle(0, 0, (int)CornerSize.X, (int)CornerSize.Y);
            TopRightCorenerSourceRec = new Rectangle((int)(OriginalTexture.Width - CornerSize.X), 0, (int)CornerSize.X, (int)CornerSize.Y);
            BottomRightCorenerSourceRec = new Rectangle((int)(OriginalTexture.Width - CornerSize.X), (int)(OriginalTexture.Height - CornerSize.Y), (int)CornerSize.X, (int)CornerSize.Y);
            BottomLeftCorenerSourceRec = new Rectangle(0, (int)(OriginalTexture.Height - CornerSize.Y), (int)CornerSize.X, (int)CornerSize.Y);
        }

        void MoveAndResizeUI()
        {
            BackgraoundRec = new Rectangle((int)(Position.X + borderThickness.Left), (int)(Position.Y + borderThickness.Top), (int)(Size.X - (borderThickness.Right * 2)), (int)(Size.Y - (borderThickness.Bottom * 2)));

            TopLineRec = new Rectangle((int)(CornerSize.X + Position.X), (int)Position.Y, (int)(Size.X - CornerSize.X * 2), borderThickness.Top);
            RightLineRec = new Rectangle((int)((Size.X - borderThickness.Right) + Position.X), (int)(CornerSize.Y + Position.Y), borderThickness.Right, (int)(Size.Y - CornerSize.Y * 2));
            BottomLineRec = new Rectangle((int)(CornerSize.X + Position.X), (int)((Size.Y - borderThickness.Bottom) + Position.Y), (int)(Size.X - CornerSize.X * 2), borderThickness.Bottom);
            LeftLineRec = new Rectangle((int)Position.X, (int)(CornerSize.Y + Position.Y), borderThickness.Left, (int)(Size.Y - CornerSize.Y * 2));

            TopLeftCorenerRec = new Rectangle((int)Position.X, (int)Position.Y, (int)CornerSize.X, (int)CornerSize.Y);
            BottomLeftCorener = new Rectangle((int)Position.X, (int)((Size.Y - CornerSize.Y) + Position.Y), (int)CornerSize.X, (int)CornerSize.Y);
            TopRightCorener = new Rectangle((int)((Size.X - CornerSize.X) + Position.X), (int)Position.Y, (int)CornerSize.X, (int)CornerSize.Y);
            BottomRightCorener = new Rectangle((int)((Size.X - CornerSize.X) + Position.X), (int)((Size.Y - CornerSize.Y) + Position.Y), (int)CornerSize.X, (int)CornerSize.Y);
        }

        void CropTextures(GraphicsDeviceManager Graphics)
        {
            BackgraoundTexture = CropTexture.Crop(Graphics, OriginalTexture, BackgraoundSourceRec);

            TopLineTexture = CropTexture.Crop(Graphics, OriginalTexture, TopLineSourceRec);
            RightLineTexture = CropTexture.Crop(Graphics, OriginalTexture, RightSourceLineRec);
            BottomLineTexture = CropTexture.Crop(Graphics, OriginalTexture, BottomSourceLineRec);
            LeftLineTexture = CropTexture.Crop(Graphics, OriginalTexture, LeftSourceLineRec);

            TopLeftCornerTexture = CropTexture.Crop(Graphics, OriginalTexture, TopLeftCorenerSourceRec);
            TopRightCornerTexture = CropTexture.Crop(Graphics, OriginalTexture, TopRightCorenerSourceRec);
            BottomRightCornerTexture = CropTexture.Crop(Graphics, OriginalTexture, BottomRightCorenerSourceRec);
            BottomLeftCornerTexture = CropTexture.Crop(Graphics, OriginalTexture, BottomLeftCorenerSourceRec);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackgraoundTexture, BackgraoundRec, Color.White * Alpha);

            spriteBatch.Draw(TopLineTexture, TopLineRec, Color.White * Alpha);
            spriteBatch.Draw(TopLineTexture, TopLineRec, Color.White * Alpha);
            spriteBatch.Draw(RightLineTexture, RightLineRec, Color.White * Alpha);
            spriteBatch.Draw(BottomLineTexture, BottomLineRec, Color.White * Alpha);
            spriteBatch.Draw(LeftLineTexture, LeftLineRec, Color.White * Alpha);

            spriteBatch.Draw(TopLeftCornerTexture, TopLeftCorenerRec, Color.White * Alpha);
            spriteBatch.Draw(TopRightCornerTexture, TopRightCorener, Color.White * Alpha);
            spriteBatch.Draw(BottomRightCornerTexture, BottomRightCorener, Color.White * Alpha);
            spriteBatch.Draw(BottomLeftCornerTexture, BottomLeftCorener, Color.White * Alpha);
        }
    }
}
