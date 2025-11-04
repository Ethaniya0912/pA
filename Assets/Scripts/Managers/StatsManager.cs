using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//플레이어관리
public class StatsManager : MonoBehaviour
{
    //최대 스탯//
    public float maxHealth = 100.0f;
    public float maxStamina = 100.00f;
    public float maxFood = 100.0f;

    //기본 스탯 (파워업 적용 전)//
    public float baseHealth = 100.0f;
    public float baseStamina = 100.0f;
    public float baseFood = 100.0f;

    // 아이템 관련 기본 스탯 //
    public float baseShield = 0.0f;
    public float baseArmor = 0.0f;
    public float baseStrength = 1.0f;
    public float baseCriticalChance = 1.0f;
    public float baseCriticalDamage = 1.5f;
    public float baseAttackSpeed = 1.0f;
    public float baseSpeed = 1.0f;
    public float baseMaxHit = 1.0f;

    public float baseLifeSteal = 0.0f;
    public float baseHealthRegen = 1f;
    public float baseStaminaRegen = 0.1f;
    public float baseItemDropRate = 0.0f;
    public float baseJumpHeight = 1.0f;
    public float baseArrowDamage = 1.0f;
    public float baseKnockbackStrength = 1.0f;

    //현재 스탯//
    public float currentHealth = 0.0f;
    public float currentStamina = 0.0f;
    public float currentFood = 0.0f;

    //아이템 관련 현재 스탯//
    public float currentShield = 0.0f;
    public float currentArmor = 0.0f;
    public float currentStrength = 0.0f;
    public float currentCriticalChance = 0.0f;
    public float currentCriticalDamage = 0.0f;
    public float currentAttackSpeed = 0.0f;
    public float currentArrowDamage = 0.0f;
    public float currentSpeed = 0.0f;
    public float currentMaxHit = 0.0f;
    public float currentLifeSteal = 0.0f;
    public float currentKnockbackStrength = 0.0f;

    //회복 관련//
    public float HpRegenRate = 0.1f; // Health Regeneration rate per second
    public float StaminaRegenRate = 0.1f; // Stamina Regeneration rate per second
    public float FoodDecreaseRate = 0.1f; // Food decrease rate per second
    //죽음 관련//
    public bool isDead = false;

    protected virtual void Awake()
    {
        //초기화
        currentHealth = Mathf.Clamp(baseHealth, 0, maxHealth);
        currentStamina = Mathf.Clamp(baseStamina, 0, maxStamina);
        currentFood = Mathf.Clamp(baseFood, 0, maxFood);
        UpdateStats();
    }

    //PlayerManager에서 호출되는 함수//
    public virtual void HandleAllStats()
    {
        RegenerateStats(); 
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    // 공격 받았 때 호출되는 함수//
    public void TakeDamage(float damage)
    {
        Debug.Log(currentHealth);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log(currentHealth);
    }

    public void RegenerateStats()
    {
        if(currentFood >= 0.0) // 포만감 상태일때 회복
        {
            //체력 회복
            currentHealth += HpRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            //스테미나 회복
            currentStamina += StaminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            //회복에 따른 포만감 감소
            currentFood -= FoodDecreaseRate * Time.deltaTime;
            currentFood = Mathf.Clamp(currentFood, 0, maxFood);
        }
    }
    // 플레이어가 죽었을 때 호출되는 함수//
    public void Die()
    {
        isDead = true;
        Debug.Log("Player has died.");
    }
    private Dictionary<PowerupType, float> passiveBonuses = new Dictionary<PowerupType, float>(); // 누적 파워업 보너스

    // 패시브 아이템 적용 함수
    public void ApplyPassiveItem(PassiveItemData item)
    {
        if (item == null || item.powerupType == PowerupType.None) return;

        if (!passiveBonuses.ContainsKey(item.powerupType))
        {
            passiveBonuses[item.powerupType] = 0.0f;
        }
        passiveBonuses[item.powerupType] += item.powerupValue; // 중복 누적
        Debug.Log($"Applied passive: {item.powerupType} +{item.powerupValue}, total = {passiveBonuses[item.powerupType]}");

        // 스탯 반영 (UpdateStats 호출)
        UpdateStats();
    }

    // 스탯 업데이트 함수
    private void UpdateStats()
    {
        Debug.Log("Updating stats with passive bonuses...");
        // 예: 파워업 보너스 적용
        maxHealth = baseHealth + GetBonus(PowerupType.MaxHP);
        maxStamina = baseStamina + GetBonus(PowerupType.MaxStamina);
        maxFood = baseFood; // 현재는 food 관련 파워업 없음
        Debug.Log($"Before HpRegenRate: {HpRegenRate}");
        Debug.Log($"Health Regen Bonus: {GetBonus(PowerupType.HealthRegen)}");
        Debug.Log($"Base Health Regen: {baseHealthRegen}");
        HpRegenRate = baseHealthRegen + GetBonus(PowerupType.HealthRegen);
        Debug.Log($"New HpRegenRate: {HpRegenRate}");
        StaminaRegenRate = baseStaminaRegen + GetBonus(PowerupType.StaminaRegen);
        currentShield = baseShield + GetBonus(PowerupType.Shield);
        currentArmor = baseArmor + GetBonus(PowerupType.Armor);
        currentStrength = baseStrength + GetBonus(PowerupType.Strength);
        currentCriticalChance = baseCriticalChance + GetBonus(PowerupType.CritChance);
        currentAttackSpeed = baseAttackSpeed + GetBonus(PowerupType.AttackSpeed);
        currentArrowDamage = baseArrowDamage + GetBonus(PowerupType.ArrowDamage);
        currentSpeed = baseSpeed + GetBonus(PowerupType.MoveSpeed);
        currentLifeSteal = baseLifeSteal + GetBonus(PowerupType.Lifesteal);
        currentCriticalDamage = baseCriticalDamage; // 현재는 크리티컬 데미지 관련 파워업 없음
        currentKnockbackStrength = baseKnockbackStrength; // 현재는 넉백 관련 파워업 없음
    }

    private float GetBonus(PowerupType type)
    {
        return passiveBonuses.ContainsKey(type) ? passiveBonuses[type] : 0.0f;
    }
}
