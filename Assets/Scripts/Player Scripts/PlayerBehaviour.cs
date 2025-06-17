using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField][Range(0f, 10f)] private float playerSpeed;
    private float turnSpeed = 360f;
    private float gravity = -9.81f;
    private float gravityMultiplier = 3f;
    private float velocity;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera isometricCam;
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;

    [Header("HUD/UI")]
    [SerializeField] private GameObject hUD;

    [Header("Animators")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator armAnimator;

    [Header("Audios")]
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioSource audioSource;

    private CharacterController controller;
    private AllInputManager allInputManager;

    public static Action shootInput;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        allInputManager = AllInputManager.Instance;
    }
    private void Update()
    {
        SwitchView();
        IsometricMovement();
        FirstPersonShoot();
        Interaction();
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
            armAnimator.Play("Fade In");
            hUD.SetActive(true);
        }
        else if (switchInputFirstPerson == 1)
        {
            firstPersonCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.m_enabled = true;
            firstPersonCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.m_enabled = true;

            allInputManager.FirstPersonToIsometricView();
            isometricCam.Priority = 1;
            firstPersonCam.Priority = 0;
            armAnimator.Play("Fade Out");
            hUD.SetActive(false);
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
            animator.Play("Walking");
        }
        else
        {
            animator.Play("Idle");
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
    private void Interaction()
    {
        float interact = allInputManager.Interact();
        if (interact == 1)
        {
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out ButtonInteract buttonInteract))
                {
                    buttonInteract.Interact();
                }
            }
        }
    }
    public void Death()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
    }

    public void PlayFootstepSound()
    {
        int selectedSound = UnityEngine.Random.Range(0, 3);
        audioSource.PlayOneShot(footsteps[selectedSound]);
    }
}
