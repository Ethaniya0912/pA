using UnityEngine;

[CreateAssetMenu(menuName = "pA/Noise Data")]
public class NoiseData : ScriptableObject
{
    public float noiseScale = 25f;
    [Range(1, 10)] public int octaves = 6;
    [Range(0f, 1f)] public float persistence = 0.5f;
    public float lacunarity = 2f;
    public int seed = 12345;
    public Vector2 offset = Vector2.zero;
}