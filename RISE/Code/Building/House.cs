using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RISE.Code.Units;

namespace RISE.Code.Building
{
    public class House : Building
    {
        public int TileViewDistance = 2;
        public House(Vector2 position) : base(BuildingType.House, position)
        {
            this.setViewDistance(Game1.PixelsPerTile * TileViewDistance);
        }

        public override void BuildingPlaced()
        {
            base.BuildingPlaced();

            Unit unit = new Unit(UnitType.Worker, this.Position + new Vector2(32, 32), 100f);
            Game1.gameObjects.Add(unit.GameObjectID, unit);
            Game1.renderables.Add(unit);
            Game1.unitManager.Units.Add(unit);
        }
    }
}
