using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//�÷��̾����
public class StatsManager : MonoBehaviour
{
    PlayerManager player;

    //�ִ� ����//
    public float maxHealth = 100.0f;
    public float maxStamina = 10.00f;
    public float maxFood = 100.0f;
    //���� ����//
    public float currentHealth = 0.0f;
    public float currentStamina = 0.0f;
    public float currentFood = 0.0f;
    //������ ���� ���� ����//
    public float currentShield = 0.0f;
    public float currentArmor = 0.0f;
    public float currentStrength = 0.0f;
    public float currentCriticalChance = 0.0f;
    public float currentAttackSpeed = 0.0f;
    public float currentSpeed = 0.0f;
    public float currentMaxHit = 0.0f;
    //ȸ�� ����//
    public float HpGenerationRate = 0.1f; // Health regeneration rate per second
    public float StaminaGenerationRate = 0.1f; // Stamina regeneration rate per second
    public float FoodDecreaseRate = 0.1f; // Food decrease rate per second
    //���� ����//
    public bool isDead = false;

    void Start()
    {
        //�ʱ�ȭ
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentFood = maxFood;
        player = GetComponent<PlayerManager>();
    }

    //PlayerManager���� ȣ��Ǵ� �Լ�//
    public void HandleAllStats()
    {
        UpdatePlayerStats();
        RegenerateStats();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    // ���� �޾� �� ȣ��Ǵ� �Լ�//
    public void TakeDamage(float damage)
    {
        Debug.Log(currentHealth);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log(currentHealth);
    }
    public void UpdatePlayerStats()
    {
        //������ ����Ҷ� ���׹̳� ����
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
        if(currentFood >= 0.0) // ������ �����϶� ȸ��
        {
            //ü�� ȸ��
            currentHealth += HpGenerationRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            //���׹̳� ȸ��
            currentStamina += StaminaGenerationRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            //ȸ���� ���� ������ ����
            currentFood -= FoodDecreaseRate * Time.deltaTime;
            currentFood = Mathf.Clamp(currentFood, 0, maxFood);
        }
    }
    // �÷��̾ �׾��� �� ȣ��Ǵ� �Լ�//
    public void Die()
    {
        isDead = true;
        Debug.Log("Player has died.");
    }

}
