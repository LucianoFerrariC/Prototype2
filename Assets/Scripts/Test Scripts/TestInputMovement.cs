using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class TestInputMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 2f;
    [SerializeField] float switchInputIsometric;
    [SerializeField] float switchInputFirstPerson;
    private PlayerInput playerInput;
    private CharacterController controller;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.IsometricView.Enable();
        playerInputActions.FirstPersonView.Shoot.performed += CheckShoot;
    }
    private void Update()
    {
        SwitchView();
        Vector2 movement = playerInputActions.IsometricView.Movement.ReadValue<Vector2>();
        Vector3 direction = new Vector3(movement.x, 0f, movement.y);
        direction = Vector3.ClampMagnitude(direction, 1f);

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
        Vector3 move = (direction * playerSpeed);
        controller.Move(move * Time.deltaTime);
    }
    public void CheckShoot(InputAction.CallbackContext context)
    {
        Debug.Log(context);
    }
    private void SwitchView()
    {
        switchInputIsometric = playerInputActions.IsometricView.SwitchView.ReadValue<float>();
        switchInputFirstPerson = playerInputActions.FirstPersonView.SwitchView.ReadValue<float>();
        if (switchInputIsometric == 1)
        {
            playerInputActions.IsometricView.Disable();
            playerInputActions.FirstPersonView.Enable();
        }
        else if (switchInputFirstPerson == 1)
        {
            playerInputActions.FirstPersonView.Disable();
            playerInputActions.IsometricView.Enable();
        }
    }
}
