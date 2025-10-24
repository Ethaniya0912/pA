using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("Debug Delete Later")]
    [SerializeField] InstantCharacterEffect effectToTest;
    [SerializeField] bool processEffect = false;

    private void Update()
    {
        if (processEffect)
        {
            // ��������ʰ� �ѹ��� ����.
            processEffect = false;
            // �ν��Ͻ��Ҷ�, �������� SO ���� ��ȭ��������.
            //InstantCharacterEffect effect = Instantiate(effectToTest);
            TakeStaminaDamage effect = Instantiate(effectToTest) as TakeStaminaDamage;
            effect.staminaDamage = 55;

            ProcessInstantEffect(effectToTest);
        }
    }
}
