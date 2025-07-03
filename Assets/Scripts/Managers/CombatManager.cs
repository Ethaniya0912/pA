using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    StatsManager statsManager;

    public void HandleAllCombat()
    {
        Attack();
    }
    private void Attack()
    {
        /*statsManager.TakeDamage(1.0f);*/
        if (InputHandler.Instance.LeftClickInput)
        {
            Debug.Log("Enemy : Attack!");
        }
    }
}
