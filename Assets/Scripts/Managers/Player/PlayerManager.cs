using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    // �÷��̾�Ŵ��� ���� �ʱ�ȭ//
    PlayerLocomotionManager playerLocomotionManager;

    protected override void Awake()
    {
        base.Awake();

        // ĳ�����̿� �÷��̾�� ���� ��.

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    protected override void Update()
    {
        base.Update();

        // ������ ����
        playerLocomotionManager.HandleAllMovement();
    }
}
