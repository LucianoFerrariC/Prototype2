using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchCamera : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private AllInputManager inputManager;
    private void Awake()
    {
        inputManager = AllInputManager.Instance;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        SwitchView();
    }
    private void SwitchView()
    {
        float switchInputIsometric = inputManager.IsometricSwitch();
        float switchInputFirstPerson = inputManager.FirstPersonSwitch();
        if (switchInputIsometric == 1)
        {

        }
        else if (switchInputFirstPerson == 1)
        {

        }
    }
}
