using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCharacterEffect : ScriptableObject
{
    // 게임내 존재하는 모든 이펙트는 ID가 존재할 것.
    // 네트워크에 사용될 것이기에 식별가능한 ID를 달아줄 것.
    [Header("Effect ID")]
    public int instantEffectID;

    public virtual void ProcessEffect(CharacterManager character)
    {

    }
}
