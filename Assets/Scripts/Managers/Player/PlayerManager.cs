using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    // 플레이어매니저 관련 초기화//
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerLocomotionManager playerLocomotionManager;
    public CombatManager combatManager;
    public PlayerStatsManager playerstatsManager;
    public PlayerInventoryManager playerinventoryManager;

    protected override void Awake()
    {
        base.Awake();

        // 캐릭터이외 플레이어만을 위한 것.

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        combatManager = GetComponent<CombatManager>();
        playerstatsManager = GetComponent<PlayerStatsManager>();
        playerinventoryManager = GetComponent<PlayerInventoryManager>();

    }

    private void Start()
    {
        InputHandler.Instance.player = this;
    }


    protected override void Update()
    {
        base.Update();

        // 움직임 조작
        playerLocomotionManager.HandleAllMovement();
        combatManager.HandleAllCombat();
        playerstatsManager.HandleAllStats();
        playerinventoryManager.HandleAllInventorys();
    }
}
