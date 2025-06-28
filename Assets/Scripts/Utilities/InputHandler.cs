using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    [SerializeField] Vector2 movementInput;

    PlayerController playercontroller;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    private void OnEnable()
    {
        if(Instance == null)
        {
            playercontroller = new PlayerController();
            playercontroller.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }
        playercontroller.Enable();
    }
}
