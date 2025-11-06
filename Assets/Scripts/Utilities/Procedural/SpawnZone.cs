using UnityEngine;

/// <summary>
/// 몹 스폰 존 (트리거 + 설정)
/// </summary>
public class SpawnZone : MonoBehaviour
{
    public int mobCount = 3;
    public float spawnRadius = 10f;
    public GameObject[] spawnedMobs;

    private void Start()
    {
        // MobSpawner에서 사용
    }
}