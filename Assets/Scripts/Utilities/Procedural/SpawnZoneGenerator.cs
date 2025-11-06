using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SpawnZoneGenerator<T> : MonoBehaviour
{
    public GameObject zonePrefab;
    public T[] entities;
    public float[] weights;
    public int nZones = 30;
    public float worldScale = 12f;
    public LayerMask terrainMask;

    protected ConsistentRandom random;
    protected List<SpawnZone> zones = new List<SpawnZone>();
    private float totalWeight;

    void Start()
    {
        GenerateZones(); // 내부 로직 분리
    }

    // 추가: WorldGenerator에서 호출용
    public void StartGeneration()
    {
        GenerateZones();
    }

    private void GenerateZones()
    {
        random = new ConsistentRandom(GameManager.Instance.seed + GetSeedOffset());
        totalWeight = weights.Sum();

        int size = MapGenerator.mapChunkSize;
        int attempts = 0;
        int maxAttempts = nZones * 3;

        while (zones.Count < nZones && attempts < maxAttempts)
        {
            attempts++;
            float x = (float)(random.NextDouble() * 2 - 1) * size / 2;
            float z = (float)(random.NextDouble() * 2 - 1) * size / 2;
            Vector3 origin = new Vector3(x, 200, z) * worldScale;

            if (Physics.Raycast(origin, Vector3.down, out var hit, 500f, terrainMask))
            {
                if (IsGrassBiome(hit.point.y))
                {
                    var zoneObj = Instantiate(zonePrefab, hit.point, Quaternion.identity);
                    var zone = zoneObj.GetComponent<SpawnZone>();
                    zone = ProcessZone(zone);
                    zones.Add(zone);
                }
            }
        }
        AddEntitiesToZone();
    }

    protected virtual int GetSeedOffset() => 300;
    protected virtual bool IsGrassBiome(float height) => height > 5f && height < 50f;

    public abstract void AddEntitiesToZone();
    public abstract SpawnZone ProcessZone(SpawnZone zone);

    protected T FindWeighted()
    {
        float roll = (float)random.NextDouble() * totalWeight;
        float sum = 0;
        for (int i = 0; i < entities.Length; i++)
        {
            sum += weights[i];
            if (roll <= sum) return entities[i];
        }
        return entities[0];
    }
}