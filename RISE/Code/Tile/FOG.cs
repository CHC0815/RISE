using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using RISE.Code.UI;
using RISE.Code.Units;

namespace RISE.Code.Tile
{
    public class FOG : GameObject
    {
        GraphicsDevice device;
        Color[] data = new Color[Game1.PixelsPerTile * Game1.PixelsPerTile];
        Color[] free = new Color[Game1.PixelsPerTile * Game1.PixelsPerTile];

        List<Tile> FreedTiles = new List<Tile>();
        
        public FOG(Vector2 position, Vector2 WorldSize, GraphicsDevice device) : base(Game1.GetGameObjectId(), createFog(WorldSize, device), position, 0f)
        {
            this.device = device;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = TextureNames.DISCOVERED;
            }
            for (int i = 0; i < free.Length; i++)
            {
                free[i] = TextureNames.FREE;
            }

            Game1.gameObjects.Add(this.GameObjectID, this);
        }
        public void RevealTile(Tile tile)
        {
            this.Texture.SetData(0, new Rectangle((int)tile.Position.X, (int)tile.Position.Y, Game1.PixelsPerTile, Game1.PixelsPerTile), data, 0, data.Length);
        }
        public void RevealTiles(Tile[] tilesToReveal)
        {
            for (int i = 0; i < tilesToReveal.Length; i++)
            {
                RevealTile(tilesToReveal[i]);
            }
        }
        public void FreeTile(Tile tile)
        {
            this.Texture.SetData(0, new Rectangle((int)tile.Position.X, (int)tile.Position.Y, Game1.PixelsPerTile, Game1.PixelsPerTile), free, 0, free.Length);
        }
        public void FreeTiles(Tile[] tilesToFree)
        {
            for (int i = 0; i < tilesToFree.Length; i++)
            {
                //actual free it on texture
                FreeTile(tilesToFree[i]);
            }
        }
        public override void Update(GameTime gameTime)
        {
            Tile[] copy = new Tile[FreedTiles.Count];
            FreedTiles.CopyTo(copy);
            List<Tile> lastFreedTiles = copy.ToList();
            FreedTiles.Clear();

            //get all gameobjects
            List<GameObject> allGOs = Game1.gameObjects.Values.ToList();
            //empty list of units
            List<Unit> units = new List<Unit>();
            List<Building.Building> buildings = new List<Building.Building>();
            foreach(GameObject g in allGOs)
            {
                //check whether it implements IhasViewDistance and whether it is a unit
                IhasViewDistance a = g as IhasViewDistance;
                Unit u = g as Unit;
                Building.Building b = g as Building.Building;
                //unit which implements IhasViewDistance, because Buildings Update itself
                if (a != null && u != null)
                    units.Add(u);
                if (a != null && b != null)
                    buildings.Add(b);
                    
            }
            //foreach unit
            foreach (Unit g in units)
            {
                //get new revealed/freed tile
                Tile[] tiles = g.RevealTiles();
                FreedTiles.AddRange(tiles);
            }

            //foreach building
            foreach(Building.Building b in buildings)
            {
                /* performance boost */
                //Tile[] tiles = b.RevealTiles();
                Tile[] tiles = b.tilesItFrees;
                if(tiles != null && tiles.Length > 0)
                    FreedTiles.AddRange(tiles);
            }
            //tiles that were no more visible
            List<Tile> oldTiles = lastFreedTiles.Except(FreedTiles).ToList();
            //tiles that became visible
            List<Tile> newTiles = FreedTiles.Except(lastFreedTiles).ToList();
            RevealTiles(oldTiles.ToArray());
            FreeTiles(newTiles.ToArray());
            base.Update(gameTime);
        }

        private static Texture2D createFog(Vector2 worldSize, GraphicsDevice device)
        {
            Texture2D texture = new Texture2D(device, (int)worldSize.X * Game1.PixelsPerTile, (int)worldSize.Y * Game1.PixelsPerTile);
            Color[] data = new Color[texture.Width * texture.Height];
            for(int i = 0; i < data.Length;i++)
            {
                data[i] = TextureNames.NOTDISCOVERED;
            }
            texture.SetData(data);
            return texture;
        }
    }
}
