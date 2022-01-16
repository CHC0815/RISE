using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.Tile
{
    public class World
    {
        public List<Tile> notWalkablesTiles = new List<Tile>();

        public List<Tile> Tiles = new List<Tile>();
        public List<Tile> revealedTiles = new List<Tile>();

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int TileWidth { get; protected set; }
        private TileGeneration tileGeneration;
        private float levelScale = 3f;
        private int offsetX = 10;
        private int offsetY = 10;
        private float heightMultiplier = 1f;

        GraphicsDevice device;
        public FOG fog;

        public static Wave[] waves = new Wave[] {
                new Wave(5613, 1f, 1f),
                new Wave(9551, 0.5f, 2f),
                new Wave(6712, 0.25f, 4f)
            };
        public static TerrainType[] terrainTypes = new TerrainType[] {
                new TerrainType("water", -0.2f, TextureNames.WATER),
                new TerrainType("sand", -0.05f, TextureNames.SAND),
                new TerrainType("berrybush", 0.0f, TextureNames.BUSH_BERRY),
                new TerrainType("bush", 0.02f, TextureNames.BUSH),
                new TerrainType("grass", 0.2f, TextureNames.GRASS),
                new TerrainType("grassthick", 0.25f, TextureNames.GRASS_THICK),
                new TerrainType("tree", 0.3f, TextureNames.TREE),
                new TerrainType("treepine", 0.45f, TextureNames.TREE_PINE),
                new TerrainType("dirt", 0.6f, TextureNames.DIRT),
                new TerrainType("hill", 0.8f, TextureNames.HILL),
                new TerrainType("mountain", 0.85f, TextureNames.MOUNTAIN)
            };

        public World(int width, int height, int tileWidth, GraphicsDevice device)
        {
            this.Width = width;
            this.Height = height;
            this.TileWidth = tileWidth;
            this.device = device;
            tileGeneration = new TileGeneration(Width, Height, heightMultiplier, waves, terrainTypes, levelScale, offsetX, offsetY);
        }
        /// <summary>
        /// generates a tileset with width * height
        /// </summary>
        public void GenerateWorld()
        {
            for (int xTileIndex = 0; xTileIndex < Width; xTileIndex++)
            {
                for (int zTileIndex = 0; zTileIndex < Height; zTileIndex++)
                {
                    Vector2 tilePosition = new Vector2(xTileIndex * TileWidth, zTileIndex * TileWidth);
                    Tile t = tileGeneration.GenerateTile(xTileIndex, zTileIndex);
                    t.Position = tilePosition;
                    t.draw = false;
                    t.update = false;
                    t.isWalkable = Walkable(t);
                    if (!t.isWalkable)
                        notWalkablesTiles.Add(t);
                    Tiles.Add(t);
                }
            }
            //more efficient than rendering many single textures
            Texture2D mainTextrue = GenerateBigTexture();
            Map map = new Map( mainTextrue, new Vector2((Width * Game1.PixelsPerTile)/2, (Height * Game1.PixelsPerTile)/2));
            map.draw = true;
            map.update = true;
            Game1.gameObjects.Add(map.GameObjectID, map);

            fog = new FOG(new Vector2((Width * Game1.PixelsPerTile)/2, (Height * Game1.PixelsPerTile)/2), new Vector2(this.Width, this.Height), device);
        }

        public bool Walkable(Tile tile)
        {
            return !(tile.type == 0 || tile.type == 9 || tile.type == 10 || tile.hasBuilding);
        }

        /// <summary>
        /// Rendering to a render target to speed up later rendering
        /// rendering one texture is more efficient than rendering width*height textures
        /// </summary>
        /// <returns></returns>
        public Texture2D GenerateBigTexture()
        {
            RenderTarget2D renderTarget = new RenderTarget2D(device, Width * Game1.PixelsPerTile, Height * Game1.PixelsPerTile);
            SpriteBatch sprite = new SpriteBatch(device);
            device.SetRenderTarget(renderTarget);
            sprite.Begin();
            for(int i = 0; i < Tiles.Count;i++)
            {
                sprite.Draw(Tiles[i].Texture, Tiles[i].Position + new Vector2(Game1.PixelsPerTile/2, Game1.PixelsPerTile / 2), Tiles[i].SourceRectangle, Tiles[i].baseColor, Tiles[i].Rotation, Tiles[i].Origin, 1.0f, SpriteEffects.None, 1);
            }
            sprite.End();
            device.SetRenderTarget(null);

            Texture2D texture = new Texture2D(device,renderTarget.Width, renderTarget.Height);
            Color[] data = new Color[renderTarget.Width * renderTarget.Height];
            renderTarget.GetData(data);
            texture.SetData(data);
            return texture;
        }
    }
}
