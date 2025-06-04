using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float turnSpeed = 360f;
    [SerializeField] private CinemachineVirtualCamera isometricCam;
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;
    private CharacterController controller;
    public List<MeshRenderer> playerBody;
    private AllInputManager inputManager;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        List<MeshRenderer> playerBody = new List<MeshRenderer>();
        inputManager = AllInputManager.Instance;
    }
    private void Update()
    {
        SwitchView();
        IsometricMovement();
    }
    private void SwitchView()
    {
        float switchInputIsometric = inputManager.IsometricSwitch();
        float switchInputFirstPerson = inputManager.FirstPersonSwitch();
        if (switchInputIsometric == 1)
        {
            firstPersonCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.m_enabled = false;
            firstPersonCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.m_enabled = false;

            inputManager.IsometricToFirstPersonView();
            isometricCam.Priority = 0;
            firstPersonCam.Priority = 1;
            playerBody[0].enabled = false;
            playerBody[1].enabled = false;
            playerBody[2].enabled = false;
        }
        else if (switchInputFirstPerson == 1)
        {
            firstPersonCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.m_enabled = true;
            firstPersonCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.m_enabled = true;

            inputManager.FirstPersonToIsometricView();
            isometricCam.Priority = 1;
            firstPersonCam.Priority = 0;
            playerBody[0].enabled = true;
            playerBody[1].enabled = true;
            playerBody[2].enabled = true;
        }
    }
    void IsometricMovement()
    {
        Vector2 movement = inputManager.IsometricMovement();
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
        controller.Move(move * Time.deltaTime);
    }
}
