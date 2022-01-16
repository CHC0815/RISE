using Microsoft.Xna.Framework;
using RISE.Code.Tile;
using RISE.Code.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.Units
{
    public class UnitManager
    {
        public List<Unit> Units;
        public UnitManager()
        {
            Units = new List<Unit>();
        }
        public void Update(GameTime gameTime, MouseRectangle mouseRectangle, World world)
        {
            for (int i = 0; i < Units.Count; i++)
                Units[i].UpdateUnit(gameTime, mouseRectangle, world, i, Units);
        }
    }
}
