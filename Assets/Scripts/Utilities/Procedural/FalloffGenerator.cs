using UnityEngine;

/// <summary>
/// 체스터 거리 기반 Falloff 맵 생성
/// 중앙은 0, 가장자리는 1 → 높이 감소
/// </summary>
public static class FalloffGenerator
{
    public static float[,] GenerateFalloffMap(int size)
    {
        var map = new float[size, size];

        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;  // [-1, 1]
                float z = j / (float)size * 2 - 1;
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(z));  // 체스터 거리
                map[i, j] = Evaluate(value);
            }

        return map;
    }

    /// <summary>
      /// Sigmoid-like 곡선으로 부드러운 감소
      /// p=3, a=2.2 → Muck 스타일 급격한 경계
    /// </summary>
    static float Evaluate(float value)
    {
        float a = 3f;
        float b = 2.2f;
        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}