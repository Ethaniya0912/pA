using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage; // �ٸ� ĳ���ͷκ��� �������� 
                                                    // �ʷ��� ��� ���⿡ �����

    [Header("Damage")]
    public float physicalDamage = 0; // (�̷��� 4������ "�Ϲ�","Ÿ��","����","���"�γ���)
    public float magicalDamage = 0;
    public float fireDamage = 0;
    public float iceDamage = 0;
    public float holyDamage = 0;

    // TD
    // Build Ups
    // ������,ȭ�������� ó�� ����� �Ҷ� �߰�. 
    // ���߿� ����� �ý����� �߰��� ����

    [Header("Final Damage")]
    private float finalDamage = 0; // ��� ����� �̷���� ���� ĳ���Ͱ� �޴� ������.
                                   // �⺻ ����, ���� ����, ����/������� ���

    [Header("Poise")]
    public float poiseDamage = 0;       // �޴� ������ �������� ���� ����� �Ѿ ���
    public bool poiseIsBroken = false;  // ������ �ִϸ��̼� ���, ���� ���·� �Ѿ��.

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayerDamageSFX = true;
    public AudioClip elementDamageSoundFX; // ������Ʈ �������� ����� �Ϲ� sfx ���� �����

    [Header("Direction Damage Take From")] // �ڿ��� ������ ��, �ִϸ��̼� ����� ������ �Ѿ����Բ� ������ϴ� ��.
    public float angleHitFrom;
    public Vector3 contactPoint;           // ��� ���� ȿ���� �߻��ϰ� �Ұ��� ����Ʈ üũ

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);
    }
}
