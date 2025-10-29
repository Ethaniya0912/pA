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
        // ������ó���� ���� ĳ���Ͱ� �ʿ��ϱ⶧����
        // ĳ���� �Ŵ����� ������ damageTarget�� �����, ������Ʈ�� �ҷ���
        CharacterManager damageTarget = other.GetComponent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            // ���� ��ų�� �ȵǵ��� �Ѵٸ� �ش� Ÿ�ٿ��� �������� ���� �� �ִ��� üũ.
            // ���� �ڿ� ������ ��찡 ����. ���� �ڿɿ����� ������ ����� ����

            // Ÿ���� �������� üũ. Ÿ�ٿ� �������� �������ʰ� block damage effect�� ���μ���.

            // Ÿ���� ������������ üũ

            // ������
            DamageTarget(damageTarget);

        }
    }

    protected virtual void DamageTarget(CharacterManager damageTarget)
    {
        // TakeDamageEffect�� �μ��Ͻ��� ���� ���� �ְ� ĳ���Ϳ��� ���μ�����.
        // �� ���Ĵ� TakeDamageEffect�� ó��.
        // ĳ���ʹ� �ټ��� �ݶ��̴�(����)�� ������ ���� ���̰�, �ѹ��� �ϳ��� �������� ���� ��.

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
