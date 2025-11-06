using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] private Renderer textureRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;

    private void Awake()
    {
        if (textureRenderer == null) Debug.LogError("MapDisplay: textureRenderer not assigned!", this);
        if (meshFilter == null) Debug.LogError("MapDisplay: meshFilter not assigned!", this);
        if (meshRenderer == null) Debug.LogError("MapDisplay: meshRenderer not assigned!", this);
    }

    public void DrawTexture(Texture2D texture)
    {
        if (textureRenderer == null || texture == null) return;

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(Mesh mesh)
    {
        if (meshFilter == null || meshRenderer == null) return;

        meshFilter.sharedMesh = mesh;

        // 충돌체 자동 추가
        var collider = meshFilter.GetComponent<MeshCollider>();
        if (collider == null) collider = meshFilter.gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = mesh;
        collider.convex = false;
    }
}