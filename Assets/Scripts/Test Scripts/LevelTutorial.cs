using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] GameObject movementPrompt;
    private AllInputManager allInputManager;

    private void Start()
    {
        allInputManager = AllInputManager.Instance;
    }
    private void Update()
    {
        Movement();
        SwitchView();
    }
    private void Movement()
    {
        Vector2 movement = allInputManager.IsometricMovement();
        if (movement != Vector2.zero)
        {
            animator.Play("Move Fade Out");
            SwitchView();
        }
    }
    private void SwitchView()
    {
        float switchView = allInputManager.IsometricSwitch();
        float shoot = allInputManager.Shoot();
        if (switchView == 1)
        {
            animator.Play("Shoot Fade In");
        }
        if (shoot == 1)
        {
            animator.Play("Shoot Fade In");
        }
    }
}
