using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.UI
{
    public class Lable
    {
        public string Text;
        private Vector2 _position;
        public Vector2 Positon
        {
            get { return _position; }
            set
            {
                //prevent blur
                _position = new Vector2((int)value.X, (int)value.Y);
            }
        }
        public Color baseColor;

        public Lable(string text, Vector2 position)
        {
            this.Positon = position;
            this.Text = text;
            baseColor = Color.Black;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(Game1.MAINFONT, this.Text, this._position, baseColor);
        }
    }
}
