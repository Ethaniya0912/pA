using UnityEngine;

/// <summary>
/// 시드 기반으로 일관된 난수를 생성하는 클래스
/// Muck의 모든 프로시저럴 요소가 동일한 시드로 재현되도록 보장
/// </summary>
public class ConsistentRandom
{
    private readonly System.Random random;

    public ConsistentRandom(int seed)
    {
        random = new System.Random(seed);
    }

    /// <summary>
    /// [0.0, 1.0) 범위의 double 반환
    /// </summary>
    public double NextDouble() => random.NextDouble();

    /// <summary>
    /// [min, max) 정수 반환
    /// </summary>
    public int Next(int min, int max) => random.Next(min, max);
}