using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    // �÷��̾�Ŵ��� ���� �ʱ�ȭ//
    public PlayerLocomotionManager playerLocomotionManager;
    public CombatManager combatManager;
    public StatsManager statsManager;

    protected override void Awake()
    {
        base.Awake();

        // ĳ�����̿� �÷��̾�� ���� ��.

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        combatManager = GetComponent<CombatManager>();
        statsManager = GetComponent<StatsManager>();

        InputHandler.Instance.player = this;

    }

    protected override void Update()
    {
        base.Update();

        // ������ ����
        playerLocomotionManager.HandleAllMovement();
        combatManager.HandleAllCombat();
        statsManager.HandleAllStats();
    }
}
