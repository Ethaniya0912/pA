using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    // 오버라이드 가능하도록 virtual로.
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
    {
        character.animator.SetFloat("Horizontal",  horizontalValue); // Horizontal 에 horizontalValue 를 넘겨주기.
        character.animator.SetFloat("Vertical", verticalValue); // Vertical 에 verticalValue 를 넘겨주기.
    }
}
