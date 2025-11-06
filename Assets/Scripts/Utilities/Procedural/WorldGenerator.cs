using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int seed = 12345;

    [Header("Generators")]
    public MapGenerator mapGen;
    public ResourceGenerator resourceGen;
    public SpawnZoneGenerator<Mob> mobGen;

    [ContextMenu("Generate World")]
    public void Generate()
    {
        // 1. 시드 설정
        GameManager.Instance.seed = seed;

        // 2. 지형 먼저 생성 (heightMap 생성됨)
        mapGen.GenerateMap();

        // 3. 자원 생성 (이제 heightMap 있음!)
        resourceGen.GenerateResources();

        // 4. 몹 존 생성
        if (mobGen != null)
            mobGen.StartGeneration();
    }

    private void Start()
    {
        Generate(); // 자동 실행
    }
}