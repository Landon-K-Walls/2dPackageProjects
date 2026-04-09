using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCUtil.Procedural
{
    public static class Procedural
    {
        public static float[,] PerlinNoiseMap(Vector2Int size, float scale, float seed)
        {
            float[,] noiseMap = new float[size.y, size.x];

            for (int z = 0; z < size.y; z++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    float sampleX = seed + x / scale;
                    float sampleZ = seed + z / scale;

                    float noise = Mathf.PerlinNoise(sampleX, sampleZ);

                    noiseMap[x, z] = noise;
                }
            }

            return noiseMap;
        }

        public static float[,] PerlinNoiseMap(Vector2Int size, float scale, float seed, NoiseOctave[] octaves)
        {
            float[,] noiseMap = new float[size.y, size.x];

            for (int z = 0; z < size.y; z++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    float sampleX = seed + x / scale;
                    float sampleZ = seed + z / scale;

                    float noise = 0;
                    for(int i = 0; i < octaves.Length; i++)
                    {
                        noise += octaves[i].amplitude *
                            Mathf.PerlinNoise(
                                octaves[i].frequency * sampleX,
                                octaves[i].frequency * sampleZ);
                    }

                    noiseMap[x, z] = noise;
                }
            }

            return noiseMap;
        }

        public static float[,] PerlinNoiseMap(Vector2Int size, float scale, float seed, Vector2 offset)
        {
            float[,] noiseMap = new float[size.y, size.x];

            for (int z = 0; z < size.y; z++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    float sampleX = seed + (x + offset.x) / scale;
                    float sampleZ = seed + (z + offset.y) / scale;

                    float noise = Mathf.PerlinNoise(sampleX, sampleZ);
                    noiseMap[x, z] = noise;
                }
            }

            return noiseMap;
        }

        public static float[,] PerlinNoiseMap(Vector2Int size, float scale, float seed, NoiseOctave[] octaves, Vector2 offset)
        {
            float[,] noiseMap = new float[size.y, size.x];

            for (int z = 0; z < size.y; z++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    float sampleX = seed +(x + offset.x) / scale;
                    float sampleZ = seed + (z + offset.y) / scale;

                    float noise = 0;
                    for (int i = 0; i < octaves.Length; i++)
                    {
                        noise += octaves[i].amplitude *
                            Mathf.PerlinNoise(
                                octaves[i].frequency * sampleX,
                                octaves[i].frequency * sampleZ);
                    }
                    noiseMap[x, z] = noise;
                }
            }

            return noiseMap;
        }

        public static int SToInt(string S)
        {
            int p = 0;
            for (int i = 0; i < S.Length; i++)
            {
                p += S[i];
            }

            return p;
        }

    }

    [System.Serializable]
    public struct NoiseOctave
    {
        public float amplitude;
        public float frequency;
    }
}
