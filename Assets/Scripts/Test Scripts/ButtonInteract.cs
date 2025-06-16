using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider doorCollider;
    [SerializeField] private bool doorTrigger;

    private void Start()
    {
        doorTrigger = false;
    }
    public void Interact()
    {
        Debug.Log("Has presionado el boton!");
        if (doorTrigger == true)
        {
            animator.Play("Opening");
            doorCollider.enabled = false;
        }
        doorTrigger = !doorTrigger;
    }
}
