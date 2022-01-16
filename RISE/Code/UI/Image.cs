using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.UI
{
    public class Image
    {
        Texture2D texture;
        Vector2 Position;
        public Image(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.Position = position;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.texture, this.Position, Color.White);
        }
    }
}
