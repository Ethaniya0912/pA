using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCharacterEffect : ScriptableObject
{
    // ���ӳ� �����ϴ� ��� ����Ʈ�� ID�� ������ ��.
    // ��Ʈ��ũ�� ���� ���̱⿡ �ĺ������� ID�� �޾��� ��.
    [Header("Effect ID")]
    public int instantEffectID;

    public virtual void ProcessEffect(CharacterManager character)
    {

    }
}
