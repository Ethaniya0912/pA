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
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 2;

    // �׼Ǻ� //
    public bool isDodge = false;

    protected override void Awake()
    {
        base.Awake();

        // �÷��̾�Ŵ��� ������Ʈ ��������.
        player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        HandleGroundMovement();
        //AttemptToDodgeAction();
    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = InputHandler.Instance.verticalInput;
        horizontalMovement = InputHandler.Instance.horizontalInput;

        // �����Ʈ Ŭ���� �ϱ�.
    }

    private void HandleGroundMovement()
    {
        // ���߿� �ִϸ��̼ǰ��� ó��.
        GetVerticalAndHorizontalInputs();

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
            //Debug.Log("0.5�̻�");
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);

        }
        else if (InputHandler.Instance.moveAmount <= 0.5f)
        {
            // �ȴ� �ӵ��� ����.
            //Debug.Log("0.5����");
            player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }


    }

    public void AttemptToDodgeAction()
    {
        isDodge = true;
    }
}
