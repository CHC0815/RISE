using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.Tile
{
    public class TileGeneration
    {
        private float levelScale;
        private TerrainType[] terrainTypes;
        private float heightMultiplier;
        private Wave[] waves;

        private int Width;
        private int Height;

        private float[,] heightMap;

        public TileGeneration(int width, int height, float heightMultiplier, Wave[] waves, TerrainType[] terrainTypes, float levelScale, int offsetX, int offsetY)
        {
            this.Width = width;
            this.Height = height;
            this.heightMultiplier = heightMultiplier;
            this.waves = waves;
            this.terrainTypes = terrainTypes;
            this.levelScale = levelScale;

            heightMap = new NoiseMapGeneration().GenerateNoiseMap(Height, Width, levelScale, offsetX, offsetY, waves);
        }

        public Tile GenerateTile(int x, int y)
        {
            TerrainType terrainType = ChooseTerrainType(heightMap[x, y]);
            Tile t = new Tile(Game1.GetGameObjectId(), Game1.textures[terrainType.TextureId], new Vector2(x, y), 0f, World.terrainTypes.ToList().IndexOf(terrainType));
            return t;
        }
        TerrainType ChooseTerrainType(float height)
        {
            // for each terrain type, check if the height is lower than the one for the terrain type
            foreach (TerrainType terrainType in terrainTypes)
            {
                // return the first terrain type whose height is higher than the generated one
                if (height < terrainType.height)
                {
                    return terrainType;
                }
            }
            return terrainTypes[0];
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public string name;
        public float height;
        public int TextureId;

        public TerrainType(string name, float height, int textureID)
        {
            this.name = name;
            this.height = height;
            this.TextureId = textureID;
        }
    }
}
