using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    // �÷��̾�Ŵ��� ���� �ʱ�ȭ//
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerLocomotionManager playerLocomotionManager;
    public CombatManager combatManager;
    public PlayerStatsManager playerStatsManager;
    public PlayerInventoryManager playerinventoryManager;

    protected override void Awake()
    {
        base.Awake();

        // ĳ�����̿� �÷��̾�� ���� ��.

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        combatManager = GetComponent<CombatManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerinventoryManager = GetComponent<PlayerInventoryManager>();

    }

    private void Start()
    {
        InputHandler.Instance.player = this;
    }


    protected override void Update()
    {
        base.Update();

        // ������ ����
        playerLocomotionManager.HandleAllMovement();
        combatManager.HandleAllCombat();
        statsManager.HandleAllStats();
        playerinventoryManager.HandleAllInventorys();
    }
}
