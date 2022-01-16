using RISE.Code.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.UI
{
    public static class ButtonFunctions
    {
        public static void LOG(string msg)
        {
            Console.WriteLine(msg);
        }
        public static void EnableBuildMenu()
        {
            Game1.currentUIManagers.Add(MenuID.BUILDMENU, Game1.UIs[MenuID.BUILDMENU]);
        }
        public static void DisableBuildMenu()
        {
            Game1.currentUIManagers.Remove(MenuID.BUILDMENU);
        }
        public static void ToggleBuildMenu()
        {
            if (Game1.currentUIManagers.Keys.Contains(MenuID.BUILDMENU))
                DisableBuildMenu();
            else
                EnableBuildMenu();
        }

        public static void ToggleDebugMode()
        {
            Game1.isDebugMode = !Game1.isDebugMode;
        }

        public static void Building(BuildingType type)
        {
            if (Game1.player.currentBuilding != null)
                return;
            Building.Building b = null;
            switch(type)
            {
                case BuildingType.TownCenter:
                    b = new TownCenter(Microsoft.Xna.Framework.Vector2.Zero);
                    break;
                case BuildingType.House:
                    b = new House(Microsoft.Xna.Framework.Vector2.Zero);
                    break;
                case BuildingType.Wall:
                    b = new Wall(Microsoft.Xna.Framework.Vector2.Zero);
                    break;
                case BuildingType.Tower:
                    b = new Tower(Microsoft.Xna.Framework.Vector2.Zero);
                    break;
                default:
                    break;
            }
            if (b != null)
            {
                Game1.player.currentBuilding = b;
                Game1.player.isBuilding = true;
                Game1.player.isPlacing = false;
                Game1.gameObjects.Add(b.GameObjectID, b);
            }
        }
    }
}
