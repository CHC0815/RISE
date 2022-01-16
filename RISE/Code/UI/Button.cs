using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RISE.Code.UI
{
    public class Button : IUIClickable
    {
        public Rectangle boundingBox;
        [Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
        public Action action;
        public bool isActive = true;
        public bool isPressed = false;
        public Lable text;
        public string Text
        {
            get
            {
                return text.Text;
            }
            set
            {
                text.Text = value;
                MeasurePosition();
            }
        }
        public Texture2D pressed;
        public Texture2D normal;

        public Vector2 Position
        {
            get
            {
                return new Vector2(boundingBox.X, boundingBox.Y);
            }
            set
            {
                boundingBox.X = (int)value.X;
                boundingBox.Y = (int)value.Y;
            }
        }
        public Vector2 Size
        {
            get
            {
                return new Vector2(boundingBox.Width, boundingBox.Height);
            }
            set
            {
                boundingBox.Width = (int)value.X;
                boundingBox.Height = (int)value.Y;
            }
        }

        public Button(Vector2 position, Vector2 size, Action action, string text, Texture2D normal, Texture2D pressed)
        {
            boundingBox = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            this.action = action;
            this.text = new Lable(text, this.Position);
            MeasurePosition();
            this.normal = normal;
            this.pressed = pressed;
        }
        public Button(Vector2 position, Vector2 size, Action action, string text)
        {
            boundingBox = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            this.action = action;
            this.text = new Lable(text, this.Position);
            MeasurePosition();
            this.normal = Game1.textures[TextureNames.Button_normal];
            this.pressed = Game1.textures[TextureNames.Button_pressed];
        }
        /// <summary>
        /// Measures the lable position assumeing it should be centered
        /// </summary>
        public void MeasurePosition()
        {
            Vector2 size = Game1.MAINFONT.MeasureString(this.Text);
            text.Positon = this.Position + new Vector2((this.Size.X - size.X)/2, (this.Size.Y - size.Y)/2);
        }
        /// <summary>
        /// Click is called if the button was clicked
        /// </summary>
        public void Click()
        {
            if (isActive && !isPressed)
            {
                action();
                isPressed = true;
            }
        }
        /// <summary>
        /// Update logic to determine button state
        /// states (stored in 'isPressed'):
        ///     pressed
        ///     released
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Point mousePos = Mouse.GetState().Position;
                if ((mousePos.X > boundingBox.X) && (mousePos.X < boundingBox.X + boundingBox.Width) && (mousePos.Y > boundingBox.Y) && (mousePos.Y < boundingBox.Y + boundingBox.Height))
                {
                    Click();
                }
                else
                {
                    isPressed = false;
                }
            }else if(Mouse.GetState().LeftButton == ButtonState.Released)
            {
                isPressed = false;
            }
        }
        /// <summary>
        /// Draws the button depending on its state
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!isPressed)
                spriteBatch.Draw(normal, Position, new Rectangle(0, 0, normal.Width, normal.Height), Color.White, 0f, Vector2.Zero, (Size.X / normal.Width), SpriteEffects.None, 1f);
            else
                spriteBatch.Draw(pressed, Position, new Rectangle(0, 0, pressed.Width, pressed.Height), Color.White, 0f, Vector2.Zero, (Size.X/pressed.Width), SpriteEffects.None, 1f);
            if(Game1.UI_debug)
                Game1.DrawRectangle(spriteBatch, this.boundingBox);
            this.text.Draw(spriteBatch, gameTime);
        }
    }
}
