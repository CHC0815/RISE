using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RISE.Code.Building
{
    public class Tower : Building
    {
        public int TileViewDistance = 7;
        public Tower(Vector2 position) : base(BuildingType.Tower, position)
        {
            this.setViewDistance(Game1.PixelsPerTile * TileViewDistance);
        }
    }
}
