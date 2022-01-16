using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RISE.Code.Building
{
    public abstract class Building : GameObject, IhasViewDistance
    {
        public Vector2 Size;
        public float maxHitPoints;
        public float currentHitPoints;

        #region viewDistance
        private float _viewDistance = 16f;
        public float getViewDistance()
        {
            return this._viewDistance;
        }
        public void setViewDistance(float value)
        {
            this._viewDistance = value;
        }
        public Tile.Tile[] tilesItFrees;
        #endregion

        public Building(BuildingType type, Vector2 position) : base(Game1.GetGameObjectId(), textureById(type), position, 0f)
        {
            Size = new Vector2(this.Texture.Width, this.Texture.Height);
            this.Origin = new Vector2(0f, 0f);
        }
        public virtual void BuildingPlaced()
        {
            Tile.Tile[] tiles = GetTilesBelow();
            foreach (Tile.Tile t in tiles)
            {
                t.hasBuilding = true;
            }
            tilesItFrees = RevealTiles();
        }
        public override void Destroy()
        {
            //free tiles
            Tile.Tile[] tiles = GetTilesBelow();
            foreach (Tile.Tile t in tiles)
            {
                t.hasBuilding = false;
            }
            //update pathfinding grid
            base.Destroy();
        }
        public static Texture2D textureById(BuildingType type)
        {
            switch(type)
            {
                case BuildingType.LumberJack:
                    return Game1.textures[TextureNames.LumberJack];
                case BuildingType.Mill:
                    return Game1.textures[TextureNames.Mill];
                case BuildingType.Mine:
                    return Game1.textures[TextureNames.Mine];
                case BuildingType.Tower:
                    return Game1.textures[TextureNames.Tower];
                case BuildingType.TownCenter:
                    return Game1.textures[TextureNames.Town_center];
                case BuildingType.Wall:
                    return Game1.textures[TextureNames.Wall];
                case BuildingType.House:
                    return Game1.textures[TextureNames.House];
                default:
                    return Game1.textures[TextureNames.LINE];
            }
        }
        public Tile.Tile[] GetTilesBelow()
        {
            int X = (int)(this.Size.X / Game1.PixelsPerTile);
            int Y = (int)(this.Size.Y / Game1.PixelsPerTile);
            Tile.Tile[] tiles = new Tile.Tile[X*Y];

            int i = 0;
            for(int x = 0; x < X;x++)
            {
                for(int y = 0; y < Y;y++)
                {
                    int index = (((int)Position.X) / Game1.PixelsPerTile + x) * Game1.world.Width + (((int)Position.Y) / Game1.PixelsPerTile + y);
                    if(index >= 0 && index < Game1.world.Tiles.Count)
                        tiles[i] = Game1.world.Tiles[index];
                    i++;
                }
            }

            return tiles;
        }
        public bool isUndergroundFree()
        {
            Tile.Tile[] tiles = GetTilesBelow();
            if (tiles.Length == 0)
                return false;
            for(int i = 0; i < tiles.Length;i++)
            {
                if (tiles[i] == null)
                    return false;
                return isTileWalkable(tiles[i]);
            }
            return true;
        }
        bool isWalkable(int x, int y)
        {
            return !(Game1.world.Tiles[x * Game1.world.Width + y].type == 0 || Game1.world.Tiles[x * Game1.world.Width + y].type == 9 || Game1.world.Tiles[x * Game1.world.Width + y].type == 10 || Game1.world.Tiles[x * Game1.world.Width + y].hasBuilding);
        }
        public bool isTileWalkable(Tile.Tile tile)
        {
            if (tile == null)
                return false;

            int x = ((int)tile.Position.X) / Game1.PixelsPerTile;
            int y = ((int)tile.Position.Y) / Game1.PixelsPerTile;
            return isWalkable(x, y);
        }
        public bool usesTile(Tile.Tile tile)
        {
            Tile.Tile[] tiles = GetTilesBelow();
            return tiles.Contains(tile);
        }

        public Tile.Tile[] RevealTiles()
        {
            int r = (int)_viewDistance;
            Tile.Tile[] tiles = new Tile.Tile[r*r*4];
            int counter = 0;
            for(int x = -r; x < r;x++)
            {
                for (int y = -r; y < r;y++)
                {
                    int index = (x + (int)this.Position.X / Game1.PixelsPerTile) * Game1.world.Width + (y + (int)this.Position.Y / Game1.PixelsPerTile);
                    if (index >= 0 && index < Game1.world.Tiles.Count)
                    {
                        Tile.Tile t = Game1.world.Tiles[index];
                        tiles[counter] = t;
                        counter++;
                    }
                }
            }
            List<Tile.Tile> revealedTiles = new List<Tile.Tile>();
            for (int i = 0; i < tiles.Length;i++)
            {
                if(tiles[i] != null)
                {
                    float distance = Vector2.Distance(this.Position, tiles[i].Position);
                    if (distance < this._viewDistance)
                    {
                        //if (!tiles[i].isRevealed)
                        revealedTiles.Add(tiles[i]);
                        tiles[i].isRevealed = true;
                    }
                }
            }
            return revealedTiles.ToArray();
        }
    }
    public enum BuildingType
    {
        TownCenter,
        LumberJack,
        Mine,
        Tower,
        Farm,
        Mill,
        Wall,
        House
    }
}
