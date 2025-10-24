using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : StatsManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();

        // 
        player = GetComponent<PlayerManager>();

    }

    public override void HandleAllStats()
    {
        base.HandleAllStats();
        UpdatePlayerStats();
    }

    public void UpdatePlayerStats()
    {
        //������ ����Ҷ� ���׹̳� ����
        if (player.playerLocomotionManager.isDodge == true)
        {
            player.playerLocomotionManager.isDodge = false;
            Debug.Log("Player : Dash");
            Debug.Log(currentStamina);
            currentStamina -= 1.0f;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }
}
