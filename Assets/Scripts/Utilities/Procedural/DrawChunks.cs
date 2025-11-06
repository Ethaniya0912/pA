using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 청크 기반 자원 활성화/비활성화 (성능 최적화)
/// </summary>
public class DrawChunks : MonoBehaviour
{
    public int nChunks = 9; // 3x3
    public float chunkSize = 100f;
    public List<GameObject>[] resources;

    private Transform player;

    private void Start()
    {
        player = Camera.main.transform;
    }

    public void InitChunks(List<GameObject>[] res)
    {
        resources = res;
    }

    public int GetChunkIndex(int x, int z)
    {
        int chunkX = x / (MapGenerator.mapChunkSize / 3);
        int chunkZ = z / (MapGenerator.mapChunkSize / 3);
        return chunkX + chunkZ * 3;
    }

    private void Update()
    {
        if (player == null || resources == null) return;

        int playerChunkX = Mathf.FloorToInt(player.position.x / chunkSize);
        int playerChunkZ = Mathf.FloorToInt(player.position.z / chunkSize);

        for (int i = 0; i < nChunks; i++)
        {
            int cx = i % 3 - 1 + playerChunkX;
            int cz = i / 3 - 1 + playerChunkZ;
            bool active = Mathf.Abs(cx - playerChunkX) <= 1 && Mathf.Abs(cz - playerChunkZ) <= 1;

            foreach (var obj in resources[i])
                obj.SetActive(active);
        }
    }
}