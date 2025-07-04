using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//플레이어관리
public class StatsManager : MonoBehaviour
{
    PlayerManager player;

    //최대 스탯//
    public float maxHealth = 100.0f;
    public float maxStamina = 10.00f;
    public float maxFood = 100.0f;
    //현재 스탯//
    public float currentHealth = 0.0f;
    public float currentStamina = 0.0f;
    public float currentFood = 0.0f;
    //아이템 관련 현재 스탯//
    public float currentShield = 0.0f;
    public float currentArmor = 0.0f;
    public float currentStrength = 0.0f;
    public float currentCriticalChance = 0.0f;
    public float currentAttackSpeed = 0.0f;
    public float currentSpeed = 0.0f;
    public float currentMaxHit = 0.0f;
    //회복 관련//
    public float HpGenerationRate = 0.1f; // Health regeneration rate per second
    public float StaminaGenerationRate = 0.1f; // Stamina regeneration rate per second
    public float FoodDecreaseRate = 0.1f; // Food decrease rate per second
    //죽음 관련//
    public bool isDead = false;

    void Start()
    {
        //초기화
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentFood = maxFood;
        player = GetComponent<PlayerManager>();
    }

    //PlayerManager에서 호출되는 함수//
    public void HandleAllStats()
    {
        UpdatePlayerStats();
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
    public void UpdatePlayerStats()
    {
        //점프나 대시할때 스테미나 감소
        if (player.playerLocomotionManager.isDodge == true)
        {
            player.playerLocomotionManager.isDodge = false;
            Debug.Log("Player : Dash");
            Debug.Log(currentStamina);
            currentStamina -= 1f;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }
    public void RegenerateStats()
    {
        if(currentFood >= 0.0) // 포만감 상태일때 회복
        {
            //체력 회복
            currentHealth += HpGenerationRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            //스테미나 회복
            currentStamina += StaminaGenerationRate * Time.deltaTime;
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

}
