using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RISE.Code.Tile
{
    public class Map : GameObject
    {
        public Map(Texture2D texture, Vector2 position) : base(Game1.GetGameObjectId(), texture, position, 0f)
        {
        }
    }
}
