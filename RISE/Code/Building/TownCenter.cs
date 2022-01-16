using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RISE.Code.Units;

namespace RISE.Code.Building
{
    public class TownCenter : Building
    {
        public int maxUnits = 8;
        public List<Unit> units = new List<Unit>();
        public int TileViewDistance = 5;
        public TownCenter(Vector2 position) : base(BuildingType.TownCenter, position)
        {
            this.setViewDistance(Game1.PixelsPerTile * TileViewDistance);
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
