using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float turnSpeed = 360f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3f;
    [SerializeField] private float velocity;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera isometricCam;
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;

    [Header("Animators")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator armAnimator;

    private CharacterController controller;
    private AllInputManager allInputManager;

    public static Action shootInput;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        allInputManager = AllInputManager.Instance;
    }
    private void Update()
    {
        SwitchView();
        IsometricMovement();
        FirstPersonShoot();
    }
    private void SwitchView()
    {
        float switchInputIsometric = allInputManager.IsometricSwitch();
        float switchInputFirstPerson = allInputManager.FirstPersonSwitch();
        if (switchInputIsometric == 1)
        {
            firstPersonCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.m_enabled = false;
            firstPersonCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.m_enabled = false;

            allInputManager.IsometricToFirstPersonView();
            isometricCam.Priority = 0;
            firstPersonCam.Priority = 1;
            animator.Play("Fade Out");
            armAnimator.Play("Fade In");
        }
        else if (switchInputFirstPerson == 1)
        {
            firstPersonCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.m_enabled = true;
            firstPersonCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.m_enabled = true;

            allInputManager.FirstPersonToIsometricView();
            isometricCam.Priority = 1;
            firstPersonCam.Priority = 0;
            animator.Play("Fade In");
            armAnimator.Play("Fade Out");
        }
    }
    private void IsometricMovement()
    {
        Vector2 movement = allInputManager.IsometricMovement();
        Vector3 direction = new Vector3(movement.x, 0f, movement.y);

        if (direction != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var skewedInput = matrix.MultiplyPoint3x4(direction);

            var relative = (transform.position + skewedInput) - transform.position;
            var rotation = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }
        Vector3 move = ((transform.forward * direction.magnitude) * playerSpeed);
        if (controller.isGrounded && velocity < 0f)
        {
            velocity = -1f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
            move.y = velocity;
        }
        controller.Move(move * Time.deltaTime);
    }
    private void FirstPersonShoot()
    {
        float shootKey = allInputManager.Shoot();
        if (shootKey == 1)
        {
            shootInput?.Invoke();
        }
    }
}
