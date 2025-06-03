using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineSwitcher : MonoBehaviour
{
    private Animator animator;
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInputActions = new PlayerInputActions();
    }
    private void SwitchState()
    {
        float switchInputIsometric = playerInputActions.IsometricView.SwitchView.ReadValue<float>();
        float switchInputFirstPerson = playerInputActions.FirstPersonView.SwitchView.ReadValue<float>();
        if (switchInputIsometric == 1)
        {
            animator.Play("First-Person Cam");
        }
        else if (switchInputFirstPerson == 1)
        {
            animator.Play("Isometric Cam");
        }
    }
}
