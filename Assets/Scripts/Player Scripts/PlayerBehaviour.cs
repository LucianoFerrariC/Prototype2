using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float turnSpeed = 360f;
    private PlayerInput playerInput;
    private CharacterController controller;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.IsometricView.Enable();
    }
    private void Update()
    {
        SwitchView();
        IsometricMovement();
    }
    private void SwitchView()
    {
        float switchInputIsometric = playerInputActions.IsometricView.SwitchView.ReadValue<float>();
        float switchInputFirstPerson = playerInputActions.FirstPersonView.SwitchView.ReadValue<float>();
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
    void IsometricMovement()
    {

        Vector2 movement = playerInputActions.IsometricView.Movement.ReadValue<Vector2>();
        Vector3 direction = new Vector3(movement.x, 0f, movement.y);
        //direction = Vector3.ClampMagnitude(direction, 1f);

        if (direction != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var skewedInput = matrix.MultiplyPoint3x4(direction);

            var relative = (transform.position + skewedInput) - transform.position;
            var rotation = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnSpeed * Time.deltaTime);
            //transform.forward = direction;
        }
        Vector3 move = ((transform.forward * direction.magnitude) * playerSpeed);
        controller.Move(move * Time.deltaTime);
    }
}
