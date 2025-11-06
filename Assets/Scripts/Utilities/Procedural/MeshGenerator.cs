using UnityEngine;

public static class MeshGenerator
{
    public static Mesh GenerateTerrainMesh(float[,] heightMap, TerrainData terrainData)
    {
        int size = heightMap.GetLength(0);
        float topLeftX = (size - 1) / -2f;
        float topLeftZ = (size - 1) / 2f;

        var meshData = new MeshData(size, size);
        int vertexIndex = 0;

        for (int z = 0; z < size; z++)
            for (int x = 0; x < size; x++)
            {
                float height = terrainData.heightCurve.Evaluate(heightMap[x, z]) * terrainData.heightMultiplier;
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, height, topLeftZ - z);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)size, z / (float)size);

                if (x < size - 1 && z < size - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + size + 1, vertexIndex + size);
                    meshData.AddTriangle(vertexIndex + size + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }

        return meshData.CreateMesh();
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;
    int triangleIndex;

    public MeshData(int width, int height)
    {
        vertices = new Vector3[width * height];
        uvs = new Vector2[width * height];
        triangles = new int[(width - 1) * (height - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex++] = a;
        triangles[triangleIndex++] = b;
        triangles[triangleIndex++] = c;
    }

    public Mesh CreateMesh()
    {
        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}