using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage; // 다른 캐릭터로부터 데미지가 
                                                    // 초래될 경우 여기에 저장됨

    [Header("Damage")]
    public float physicalDamage = 0; // (미래에 4종류인 "일반","타격","베기","찌르기"로나눔)
    public float magicalDamage = 0;
    public float fireDamage = 0;
    public float iceDamage = 0;
    public float holyDamage = 0;

    // TD
    // Build Ups
    // 포이즌,화염데미지 처럼 빌드업 할때 추가. 
    // 나중에 빌드업 시스템을 추가후 적용

    [Header("Final Damage")]
    private float finalDamage = 0; // 모든 계산이 이루어진 이후 캐릭터가 받는 데미지.
                                   // 기본 방어력, 갑옷 방어력, 버프/디버프등 계산

    [Header("Poise")]
    public float poiseDamage = 0;       // 받는 포이즈 데미지가 가진 포이즈를 넘어설 경우
    public bool poiseIsBroken = false;  // 포이즈 애니메이션 재생, 스턴 상태로 넘어가기.

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayerDamageSFX = true;
    public AudioClip elementDamageSoundFX; // 엘레멘트 데미지가 존재시 일반 sfx 위에 사용함

    [Header("Direction Damage Take From")] // 뒤에서 때렸을 시, 애니메이션 재생을 앞으로 넘어지게끔 해줘야하는 등.
    public float angleHitFrom;
    public Vector3 contactPoint;           // 어디서 혈흔 효과가 발생하게 할건지 포인트 체크

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);
    }
}
