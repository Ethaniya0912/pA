using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : LocomotionManager
{
    PlayerManager player;
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;

    private Vector3 moveDirection;
    // �Ʒ� speed �� ���� �ִϸ��̼��� ������.
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;

    protected override void Awake()
    {
        base.Awake();

        // �÷��̾�Ŵ��� ������Ʈ ��������.
        player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        // moveDirection ���Ϳ� ������ �����ϱ�.
        // �������� ī�޶��� ����Ű�� ������ ����, ��ǲ�� ���Կ� ���� ������.
        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize(); // ����ȭ
        moveDirection.y = 0;

        // ��ǲ�ڵ鷯���� �ν��Ͻ��� moveAmount�� 0.5f �̻��� �Ǿ��� ���
        if (InputHandler.Instance.moveAmount > 0.5f)
        {
            // �޸��� �ӵ��� ����.

        }
        else if (InputHandler.Instance.moveAmount >= 0.5f)
        {
            // �ȴ� �ӵ��� ����.
        }
    }
}
