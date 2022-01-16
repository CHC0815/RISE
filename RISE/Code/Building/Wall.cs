using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RISE.Code.Building
{
    public class Wall : Building
    {
        public int TileViewDistance = 3;
        public Wall(Vector2 position) : base(BuildingType.Wall, position)
        {
            this.setViewDistance(Game1.PixelsPerTile * TileViewDistance);
        }
    }
}
