using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public float maxHp = 10f;
    public float currentHealth = 0;
    void Start()
    {
        currentHealth = maxHp;
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
    }
    public void RegenerateStats()
    {
        //시간마다 체력 증가
    }
    public void Die()
    {
        Debug.Log("Player has died.");
    }

}
