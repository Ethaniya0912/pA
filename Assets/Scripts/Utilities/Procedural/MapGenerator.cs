using UnityEditor.AssetImporters;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, Mesh, FalloffMap, ColorMap }
    public DrawMode drawMode;

    [Header("Data")]
    public TerrainData terrainData;
    public NoiseData noiseData;
    public TextureData textureData;

    [Header("Rendering")]
    public Material terrainMaterial;
    [SerializeField] private MapDisplay display;

    public const int mapChunkSize = 257;
    private float[,] heightMap;
    public int levelOfDetail;

    public float waterLevel = 0.35f; // 정규화된 높이

    private void Awake()
    {
        if (display == null) Debug.LogError("display not assigned", this);
    }

    [ContextMenu("Generate Map")]
    public void GenerateMap()
    {
        // 1. 노이즈 생성
        var noiseMap = Noise.GenerateNoiseMap(
            mapChunkSize, noiseData.seed, noiseData.noiseScale,
            noiseData.octaves, noiseData.persistence, noiseData.lacunarity, noiseData.offset);

        // 2. Falloff 적용
        if (terrainData.useFalloff)
        {
            var falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
            for (int z = 0; z < mapChunkSize; z++)
                for (int x = 0; x < mapChunkSize; x++)
                {
                    noiseMap[x, z] = Mathf.Clamp01(noiseMap[x, z] - falloffMap[x, z]);
                }
        }

        heightMap = noiseMap;

        // 3. 시각화
        display.DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap, terrainData));
        display.DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
        display.DrawTexture(TextureGenerator.ColorTextureFromHeightMap(heightMap, textureData));
        /*
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap, terrainData));
        }
        else if (drawMode == DrawMode.FalloffMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.ColorTextureFromHeightMap(heightMap, textureData));
        }*/

    }

    public float[,] GetHeightMap() => heightMap;
}