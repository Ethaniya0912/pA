using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    // �������̵� �����ϵ��� virtual��.
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
    {
        character.animator.SetFloat("Horizontal",  horizontalValue); // Horizontal �� horizontalValue �� �Ѱ��ֱ�.
        character.animator.SetFloat("Vertical", verticalValue); // Vertical �� verticalValue �� �Ѱ��ֱ�.
    }
}
