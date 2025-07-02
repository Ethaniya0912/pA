using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public void Attack()
    {
        statsManager.TakeDamage(1.0f);
        Debug.Log("Enemy : Attack!");
    }
}
