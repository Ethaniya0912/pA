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
            // 계속하지않고 한번만 해줌.
            processEffect = false;
            // 인스턴스할때, 오리지널 SO 값은 변화되지않음.
            //InstantCharacterEffect effect = Instantiate(effectToTest);
            TakeStaminaDamage effect = Instantiate(effectToTest) as TakeStaminaDamage;
            effect.staminaDamage = 55;

            ProcessInstantEffect(effectToTest);
        }
    }
}
