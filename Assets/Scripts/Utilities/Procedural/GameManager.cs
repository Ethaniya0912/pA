using UnityEngine;

/// <summary>
/// 傈开 矫靛 包府 + 教臂沛
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int seed = 12345;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static int GetSeed() => Instance.seed;
}