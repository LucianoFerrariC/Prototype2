using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip buttonSound;

    [Header("Door")]
    [SerializeField] private BoxCollider doorCollider;
    [SerializeField] private AudioSource doorAudio;
    [SerializeField] private AudioClip doorSound;

    private bool wasDoorOpened;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        wasDoorOpened = false;
    }
    public void Interact()
    {
        if (wasDoorOpened == false)
        {
            Debug.Log("Has presionado el boton!");
            animator.Play("Opening");
            audioSource.PlayOneShot(buttonSound);
            doorAudio.PlayOneShot(doorSound);
            doorCollider.enabled = false;
            wasDoorOpened = true;
        }
    }
}
