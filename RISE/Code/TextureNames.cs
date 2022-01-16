using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE
{
    public static class TextureNames
    {
        //COLORS
        public static Color NOTDISCOVERED = Color.FromNonPremultiplied(0, 0, 0, 255);
        public static Color DISCOVERED = Color.FromNonPremultiplied(0, 0, 0, 150);
        public static Color FREE = Color.Transparent;

        //UTIL
        public const int MOUSE_RECTANGLE = -3;
        public const int QUAD = -2;
        public const int LINE = -1;
        //LOGO
        public const int RISELOGO = 0;
        //TILES
        public const int TREE = 1;
        public const int TREE_PINE = 2;
        public const int GRASS_THICK = 3;
        public const int BUSH = 4;
        public const int STONE = 5;
        public const int BUSH_BERRY = 6;
        public const int WATER = 7;
        public const int GRASS = 8;
        public const int SAND = 9;
        public const int DIRT = 10;
        public const int HILL = 11;
        public const int MOUNTAIN = 12;

        //UNITS
        public const int WorkerNormal = 13;
        public const int WorkerHovered = 14;
        public const int WorkerSelected = 15;

        //UI
        public const int Button_normal = 101;
        public const int Button_pressed = 102;

        //Buildings
        public const int Town_center = 501;
        public const int House = 502;
        public const int LumberJack = 503;
        public const int Mine = 504;
        public const int Farm = 505;
        public const int Dock = 506;
        public const int Tower = 507;
        public const int Wall = 508;
        public const int Mill = 509;

        //Entities
        public const int Arrow = 1001;
    }
}
