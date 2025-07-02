using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    [SerializeField] Vector2 movementInput;

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
/*        }*/
        Debug.Log("Instance is not null");
        playerController.Enable();
    }
}
