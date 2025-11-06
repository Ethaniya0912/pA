using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeightedSpawn
{
    public GameObject prefab;
    [Range(0f, 100f)] public float weight = 1f;
}

public class ResourceGenerator : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public DrawChunks drawChunks;
    public WeightedSpawn[] resourcePrefabs;
    public NoiseData resourceNoise;
    public LayerMask terrainMask;

    [Header("Spawn Settings")]
    public int density = 2;
    public float spawnThreshold = 0.5f;
    public float minHeight = 0.35f, maxHeight = 0.65f;
    public int minSpawnAmount = 20;
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    public Vector3 rotationRange = new Vector3(10, 360, 10);

    private ConsistentRandom random;
    private List<GameObject>[] resources;
    private float totalWeight;


    List<Vector3> debugRayPositions = new List<Vector3>();
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;
        foreach (var pos in debugRayPositions)
        {
            Gizmos.DrawLine(pos, pos + Vector3.down * 1000);
        }
    }


    public void GenerateResources()
    {
        Debug.Log("Generating resources...");
        var heightMap = mapGenerator.GetHeightMap();
        var noiseMap = Noise.GenerateNoiseMap(MapGenerator.mapChunkSize, GameManager.Instance.seed + 200,
            resourceNoise.noiseScale, resourceNoise.octaves, resourceNoise.persistence, resourceNoise.lacunarity, Vector2.zero);

        int size = heightMap.GetLength(0);
        Debug.Log($"HeightMap size: {size}x{size}");
        resources = new List<GameObject>[drawChunks.nChunks];
        Debug.Log($"Initialized resources array with {resources.Length} chunks.");
        for (int i = 0; i < resources.Length; i++) resources[i] = new List<GameObject>();

        int spawned = 0;
        int attempts = 0;

        if (mapGenerator == null)
        {
            Debug.LogError("ResourceGenerator: mapGenerator is null!");
            return;
        }
        if (heightMap == null)
        {
            Debug.LogError("ResourceGenerator: heightMap is null! Did you call MapGenerator.GenerateMap() first?");
            return;
        }

        while (spawned < minSpawnAmount && attempts < 1000)
        {
            attempts++;
            for (int z = 0; z < size; z += density)
                for (int x = 0; x < size; x += density)
                {
                    float height = heightMap[x, z];
                    if (height < minHeight || height > maxHeight) continue;
                    if (noiseMap[x, z] < spawnThreshold) continue;

                    Vector3 worldPos = new Vector3(x - size / 2, 200, z - size / 2) * 12f;
                    // 스폰할 때마다
                    //debugRayPositions.Add(worldPos);
                    if (Physics.Raycast(worldPos, Vector3.down, out var hit, 1000f, terrainMask))
                    {
                        var obj = SpawnResource(hit.point);
                        int chunk = drawChunks.GetChunkIndex(x, z);
                        resources[chunk].Add(obj);
                        obj.SetActive(false);
                        spawned++;

                    }

                }
        }
    }

    GameObject SpawnResource(Vector3 pos)
    {
        //Debug.Log($"Spawning resource at position: {pos}");
        var prefab = FindWeighted(resourcePrefabs);
        var obj = Instantiate(prefab, pos, Quaternion.Euler(
            Random.Range(-rotationRange.x, rotationRange.x),
            Random.Range(0, 360),
            Random.Range(-rotationRange.z, rotationRange.z)));

        float scale = Random.Range(scaleRange.x, scaleRange.y);
        obj.transform.localScale = Vector3.one * scale;

        Debug.Log($"Spawned resource: {prefab.name} at {pos} with scale {scale}");

        return obj;
    }

    GameObject FindWeighted(WeightedSpawn[] array)
    {
        float roll = (float)random.NextDouble() * totalWeight;
        float sum = 0;
        foreach (var w in array)
        {
            sum += w.weight;
            if (roll <= sum) return w.prefab;
        }
        return array[0].prefab;
    }
    
}