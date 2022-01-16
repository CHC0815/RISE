using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RISE.Code.UI
{
    public class UIManager
    {
        public List<Button> buttons = new List<Button>();
        public List<Lable> lables = new List<Lable>();
        public List<Image> images = new List<Image>();

        public UIManager()
        {

        }
        public void Update(GameTime gameTime)
        {
            foreach(Button btn in buttons)
            {
                btn.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            foreach(Button btn in buttons)
            {
                btn.Draw(spritebatch, gameTime);
            }
            foreach(Lable lable in lables)
            {
                lable.Draw(spritebatch, gameTime);
            }
            foreach(Image img in images)
            {
                img.Draw(spritebatch, gameTime);
            }
        }
    }
}
