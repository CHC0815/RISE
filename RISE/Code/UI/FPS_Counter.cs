using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RISE.Code.UI
{
    public class FPS_Counter : Lable
    {
        private FrameCounter _frameCounter = new FrameCounter();
        public FPS_Counter(string text, Vector2 position) : base(text, position)
        {
            this.baseColor = Color.White;
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);
            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
            Text = fps;
            base.Draw(spriteBatch, gameTime);
        }
    }
}
