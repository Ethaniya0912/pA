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
    // 아래 speed 에 따라 애니메이션이 결정됨.
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 2;

    // 액션불 //
    public bool isDodge = false;

    protected override void Awake()
    {
        base.Awake();

        // 플레이어매니저 컴포넌트 가져오기.
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

        // 무브번트 클램핑 하기.
    }

    private void HandleGroundMovement()
    {
        // 나중에 애니메이션관련 처리.
        GetVerticalAndHorizontalInputs();

        // moveDirection 벡터에 움직임 산입하기.
        // 움직임은 카메라이 가리키는 방향을 기준, 인풋의 산입에 의해 결정됨.
        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize(); // 정규화
        moveDirection.y = 0;

        // 인풋핸들러에서 인스턴스의 moveAmount가 0.5f 이상이 되었을 경우
        if (InputHandler.Instance.moveAmount > 0.5f)
        {
            // 달리는 속도로 설정.
            //Debug.Log("0.5이상");
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);

        }
        else if (InputHandler.Instance.moveAmount <= 0.5f)
        {
            // 걷는 속도로 설정.
            //Debug.Log("0.5수준");
            player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }


    }

    public void AttemptToDodgeAction()
    {
        isDodge = true;
    }
}
