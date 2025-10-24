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
        // �⺻ ���׹̳� �������� �ٸ� �÷��̾��� ����Ʈ/������̾�� ����.
        Debug.Log("Character is Taking " + staminaDamage + " StaminaDamage");
        character.statsManager.currentStamina -= staminaDamage;
    }
}
