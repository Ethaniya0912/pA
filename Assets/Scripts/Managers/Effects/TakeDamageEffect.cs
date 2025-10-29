using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/TakeDamge")]
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
    public float angleHitFrom;             // 방향별 데미지 애니메이션 플레이
    public Vector3 contactPoint;           // 어디서 혈흔 효과가 발생하게 할건지 포인트 체크

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        // 캐릭터가 죽으면, 데미지 이펙트가 더 진행되지 않도록 해줌.
        if (character.isDead)
            return;

        // "무적 상태" 인지 확인. 이때는 이펙트 프로세스를 안해주니.

        CalculateDamage(character);
        // 데미지 계산
        // 방향 데미지가 어디서 오는지 체크
        // 데미지 애니메이션 실행
        // 빌드업 체크 (독, 피 etc)
        // 데미지 sfx 실행
        // 데미지 sfx (혈흔등) 실행

        // 만약 캐릭터가 ai 라면, 데미지를 초래한 캐릭터가 존재한다면 새 타겟으로 정할건지 체크.
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (characterCausingDamage != null)
        {
            // 데미지 모디파이어를 체크하고, 기본 데미지를 수정함(물리/엘레멘탈 데미지등)
        }

        // 캐릭터의 플랫 디펜스를 체크하고 데미지를 감소시킴.

        // 캐릭터의 아머 흡수를 체크하고, 데미지로부터 퍼센티지를 제함.

        // 모든 데미지 타입을 더하고, 파이널 데미지로 추가.
        finalDamage = Mathf.RoundToInt(physicalDamage + magicalDamage + fireDamage + iceDamage + holyDamage);

        if (finalDamage <= 0)
        {
            //마지막 데미지가 0보다 작거나 같을 시, 최소한 데미지인 1은 산입함.
            finalDamage = 1;
        }

        character.statsManager.currentHealth -= finalDamage;

        // 캐릭터가 스턴될지 결정하는 포이즈 데미지를 계산함
        // 포이즈가 깨질 시 데미지 애니메이션 재생
    }
}
