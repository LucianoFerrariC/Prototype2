using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] GameObject movementPrompt;

    private AllInputManager allInputManager;
    private bool wasTutorialFinised;

    private void Start()
    {
        allInputManager = AllInputManager.Instance;
        wasTutorialFinised = false;
    }
    private void Update()
    {
        if (wasTutorialFinised == false)
        {
            Movement();
            SwitchView();
            Shooting();
        }

    }
    private void Movement()
    {
        Vector2 movement = allInputManager.IsometricMovement();
        if (movement != Vector2.zero)
        {
            animator.Play("Move Fade Out");
        }
    }
    private void SwitchView()
    {
        float switchView = allInputManager.IsometricSwitch();

        if (switchView == 1)
        {
            animator.Play("Shoot Fade In");
        }
    }
    private void Shooting()
    {
        float shoot = allInputManager.Shoot();
        if (shoot ==1)
        {
            animator.Play("Reload");
        }
    }
    public void TutorialFinished()
    {
        wasTutorialFinised = true;
    }
}
