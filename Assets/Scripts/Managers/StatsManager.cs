using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public float maxHp = 10f;
    public float currentHealth = 0f;
    public float maxStamina = 10f;
    public float currentStamina = 0f;
    public float HpGenerationRate = 0.01f; // Health regeneration rate per second
    public float StaminaGenerationRate = 0.01f; // Stamina regeneration rate per second
    public bool isDead = false;
    void Start()
    {
        currentHealth = maxHp;
        currentStamina = maxStamina;
    }

    public void HandleAllStats()
    {
        UpdatePlayerStats();
        RegenerateStats();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void TakeDamage(float damage)
    {
        Debug.Log(currentHealth);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHp);
        Debug.Log(currentHealth);
    }
    public void UpdatePlayerStats()
    {
        //점프나 대시할때 스테미나 감소
        if (InputHandler.Instance.LeftClickInput)
        {
            Debug.Log("Player : Dash");
            Debug.Log(currentStamina);
            currentStamina -= 1f;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }
    public void RegenerateStats()
    {
        currentHealth += HpGenerationRate * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHp);
        currentStamina += StaminaGenerationRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }
    public void Die()
    {
        isDead = true;
        Debug.Log("Player has died.");
    }

}
