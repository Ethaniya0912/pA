using UnityEngine;

[CreateAssetMenu(menuName = "pA/Texture Data")]
public class TextureData : ScriptableObject
{
    [System.Serializable]
    public class Layer
    {
        public Color tint = Color.white;
        [Range(0f, 1f)] public float startHeight = 0.1f;
    }

    public Layer[] layers;
}