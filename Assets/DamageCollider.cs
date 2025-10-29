using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lighteningDamage = 0;
    public float holyDamage = 0;

    [Header("Contact Point")]
    protected Vector3 contactPoint;

    [Header("Character Damaged")]
    protected List<CharacterManager> characterDamaged = new List<CharacterManager>();

    private void OnTriggerEnter(Collider other)
    {
        // 데미지처리를 해줄 캐릭터가 필요하기때문에
        // 캐릭터 매니저의 변수인 damageTarget을 만들고, 컴포넌트를 불러옴
        CharacterManager damageTarget = other.GetComponent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            // 만약 팀킬이 안되도록 한다면 해당 타겟에게 데미지를 입힐 수 있는지 체크.
            // 가령 코옵 게임일 경우가 있음. 물론 코옵에서도 데미지 허용이 가능

            // 타겟이 블럭중인지 체크. 타겟에 데미지를 가하지않고 block damage effect를 프로세싱.

            // 타겟이 무적상태인지 체크

            // 데미지
            DamageTarget(damageTarget);

        }
    }

    protected virtual void DamageTarget(CharacterManager damageTarget)
    {
        // TakeDamageEffect를 인서턴스한 이후 값을 주고 캐릭터에서 프로세스함.
        // 그 이후는 TakeDamageEffect가 처리.
        // 캐릭터는 다수의 콜라이더(사지)를 가지고 있을 것이고, 한번에 하나만 데미지를 입힐 것.

        if (characterDamaged.Contains(damageTarget))
            return;

        characterDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicalDamage = magicDamage;
        damageEffect.iceDamage = fireDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.holyDamage = holyDamage;

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }
}
