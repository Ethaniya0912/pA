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
        //������ ����Ҷ� ���׹̳� ����
    }
    public void RegenerateStats()
    {
        //�ð����� ü�� ����
    }
    public void Die()
    {
        Debug.Log("Player has died.");
    }

}
