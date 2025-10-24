using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // ��ǲ�ڵ鷯 �ν��Ͻ� �̱���
    public static InputHandler Instance;

    // ���� �÷��̾�
    public PlayerManager player;

    //�÷��̾� ������//
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;
    public bool isMoving;

    //�÷��̾� �׼�//
    public bool DashInput = false;
    public bool LeftClickInput;
    public bool TabInput;
    public bool InteractInput; //EŰ

    // ��ǲ �׼Ǹ�
    PlayerControls playerController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Instance.enabled = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    private void OnEnable()
    {
        /*        if(Instance == null)
                {*/
        Debug.Log("Instance is Null");
        playerController = new PlayerControls();
        playerController.PlayerControlz.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        playerController.PlayerActions.Dash.performed += i => DashInput = true;
        playerController.PlayerActions.LeftClick.performed += i => LeftClickInput = true;
        playerController.PlayerActions.Tab.performed += i => TabInput = true;
        playerController.PlayerActions.Interaction.performed += i => InteractInput = true;
        /*        }*/
        Debug.Log("Instance is not null");
        playerController.Enable();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleLeftClickInput();
        HandleDashInput();
        HandleTabInput();
        HandleInteraction();
    }

    private void HandleMovementInput()
    {
        // movementInput ���Ͱ����� x,y ���� �� ��ǲ�� ����
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        // ���밪�� �����ְ� ��.
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) * Mathf.Abs(horizontalInput));

        // ������� 0.5, 1�� Ŭ���� ����.
        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount < 1)
        {
            moveAmount = 1;
        }

        player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput);

        isMoving = (verticalInput != 0.0f || horizontalInput != 0.0f); 
    }

    // �׼ǰ��� //
    private void HandleDashInput()
    {
        if (DashInput)
        {
            DashInput = false;
            player.playerLocomotionManager.AttemptToDodgeAction();
        }

    }

    private void HandleLeftClickInput()
    {
        if (LeftClickInput)
        {
            //Debug.Log("LeftClicked!");
            LeftClickInput = false;
            // TODO: UI �����찡 �����ִٸ�, �ƹ��͵� ����.

        }
    }

    private void HandleTabInput()
    {
        if (TabInput)
        {
            //Debug.Log("TabPressed!");
            TabInput = false;
        }
    }
    private void HandleInteraction()
    {
        if (InteractInput)
        {
            InteractInput = false;
        }
    }
}