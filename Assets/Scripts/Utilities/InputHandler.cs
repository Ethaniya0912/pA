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
    [SerializeField] float verticalInput;
    [SerializeField] float horizontalInput;
    public float moveAmount;

    //�÷��̾� �׼�//
    [SerializeField] bool LeftClickInput;

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
            playerController.PlayerActions.LeftClick.performed += i => LeftClickInput = true;
/*        }*/
        Debug.Log("Instance is not null");
        playerController.Enable();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleLeftClickInput();
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

        
    }

    private void HandleLeftClickInput()
    {
        if (LeftClickInput)
        {
            LeftClickInput = false;

            // TODO: UI �����찡 �����ִٸ�, �ƹ��͵� ����.
        }
    }
}
