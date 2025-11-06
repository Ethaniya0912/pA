using UnityEngine;

/// <summary>
/// Octave 기반 Perlin Noise 생성
/// Muck의 자연스러운 지형 형성을 위한 핵심 알고리즘
/// </summary>
public static class Noise
{
    /// <summary>
    /// size x size 크기의 노이즈 맵 생성
    /// </summary>
    public static float[,] GenerateNoiseMap(
        int size,
        int seed,
        float scale,
        int octaves,
        float persistence,
        float lacunarity,
        Vector2 offset)
    {
        var map = new float[size, size];
        var random = new ConsistentRandom(seed);

        // 여러 octave를 위한 시드 오프셋
        var octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = random.Next(-100000, 100000) + offset.x;
            float offsetZ = random.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetZ);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int z = 0; z < size; z++)
            for (int x = 0; x < size; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - size / 2 + octaveOffsets[i].x) / scale * frequency;
                    float sampleZ = (z - size / 2 + octaveOffsets[i].y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                map[x, z] = noiseHeight;

                if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
            }

        // 정규화: [-1, 1] → [0, 1]
        for (int z = 0; z < size; z++)
            for (int x = 0; x < size; x++)
            {
                map[x, z] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, map[x, z]);
            }

        return map;
    }
}