using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RISE.Code.Tile
{
    public class Tile : GameObject
    {
        public float Height;
        public int type;

        public bool isRevealed = false;

        public bool hasBuilding = false;

        public bool isWalkable = false;

        public Tile(int id, Texture2D texture, Vector2 position, float rotation, int typeIndex) : base(id, texture, position, 0f)
        {
            type = typeIndex;
        }

        public override void Start()
        {
            base.Start();
        }
        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
