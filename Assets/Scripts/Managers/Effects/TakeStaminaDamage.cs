using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
public class TakeStaminaDamage : InstantCharacterEffect
{
    public float staminaDamage;

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);
        CalculateStaminaDamage(character);
    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
        // 기본 스테미나 데미지를 다른 플레이어의 이펙트/모디파이어와 비교함.
        Debug.Log("Character is Taking " + staminaDamage + " StaminaDamage");
        character.statsManager.currentStamina -= staminaDamage;
    }
}
