using UnityEngine;

/// <summary>
/// 높이맵 → 텍스처 변환 유틸리티
/// Muck 스타일 에디터 미리보기용
/// </summary>
public static class TextureGenerator
{
    /// <summary>
    /// 높이맵을 흑백 텍스처로 변환 (NoiseMap)
    /// </summary>
    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] pixels = new Color[width * height];
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float value = heightMap[x, z];
                pixels[z * width + x] = Color.Lerp(Color.black, Color.white, value);
            }
        }

        return CreateTexture(pixels, width, height);
    }

    /// <summary>
    /// 높이맵을 컬러 텍스처로 변환 (바이옴 기반)
    /// </summary>
    public static Texture2D ColorTextureFromHeightMap(float[,] heightMap, TextureData textureData)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] pixels = new Color[width * height];
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float heightValue = heightMap[x, z]; // Y축 반전
                pixels[z * width + x] = GetBiomeColor(heightValue, textureData);
            }
        }

        return CreateTexture(pixels, width, height);
    }

    private static Color GetBiomeColor(float height, TextureData data)
    {
        for (int i = data.layers.Length - 1; i >= 0; i--)
        {
            if (height > data.layers[i].startHeight)
                return data.layers[i].tint;
        }
        return data.layers[0].tint;
    }

    private static Texture2D CreateTexture(Color[] pixels, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Bilinear; // 필터 모드 설정(Bilinear, Point, Trilinear)
        texture.mipMapBias = 0f; // MipMap 강도 조절
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}