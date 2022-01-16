using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.Tile
{
    public class NoiseMapGeneration
    {
        private float offset = 0.1f;
        public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale, float offsetX, float offsetZ, Wave[] waves)
        {
            float[,] noiseMap = new float[mapDepth, mapWidth];

            for(int zIndex = 0; zIndex < mapDepth; zIndex++)
            {
                for(int xIndex = 0; xIndex < mapWidth; xIndex++)
                {
                    float sampleX = (xIndex + offsetX) / scale;
                    float sampleZ = (zIndex + offsetZ) / scale;

                    float noise = 0f;
                    float normalization = 0f;
                    foreach(Wave wave in waves)
                    {
                        noise += wave.amplitude * PerlinNoise(sampleX * wave.frequency + wave.Seed, sampleZ * wave.frequency + wave.Seed);
                        normalization += wave.amplitude;
                    }
                    noise /= normalization;

                    noiseMap[zIndex, xIndex] = noise + offset;
                }
            }
            return noiseMap;
        }
        public float PerlinNoise(float x, float y)
        {
            return Perlin.Noise(x, y);
        }
    }
    [System.Serializable]
    public class Wave
    {
        public float Seed;
        public float frequency;
        public float amplitude;

        public Wave(float seed, float frequency, float amplitude)
        {
            this.Seed = seed;
            this.frequency = frequency;
            this.amplitude = amplitude;
        }
    }
}
