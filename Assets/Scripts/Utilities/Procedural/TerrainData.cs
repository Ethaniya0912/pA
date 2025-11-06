using UnityEngine;

[CreateAssetMenu(menuName = "pA/Terrain Data")]
public class TerrainData : ScriptableObject
{
    public float uniformScale = 2.5f;
    public bool useFalloff = true;
    public float heightMultiplier = 30f;
    public AnimationCurve heightCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    public float minHeight => uniformScale * heightMultiplier * heightCurve.Evaluate(0f);
    public float maxHeight => uniformScale * heightMultiplier * heightCurve.Evaluate(1f);
}