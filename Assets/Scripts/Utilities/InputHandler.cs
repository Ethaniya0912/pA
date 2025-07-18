using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // 인풋핸들러 인스턴스 싱글턴
    public static InputHandler Instance;

    // 로컬 플레이어
    public PlayerManager player;

    //플레이어 움직임//
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    //플레이어 액션//
    public bool DashInput = false;
    public bool LeftClickInput;
    public bool TabInput;

    // 인풋 액션맵
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
    }

    private void HandleMovementInput()
    {
        // movementInput 벡터값에서 x,y 값을 각 인풋에 대입
        verticalInput = movementInput.y; 
        horizontalInput = movementInput.x;

        // 절대값을 돌려주게 함.
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) * Mathf.Abs(horizontalInput));

        // 밸류값을 0.5, 1로 클램프 해줌.
        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount < 1)
        {
            moveAmount = 1;
        }

        player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput);
    }

    // 액션관련 //
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
            Debug.Log("LeftClicked!");
            LeftClickInput = false;
            // TODO: UI 윈도우가 열려있다면, 아무것도 안함.

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

}
