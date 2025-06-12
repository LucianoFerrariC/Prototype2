using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public bool playerDetected;
    private void Awake()
    {
        playerDetected = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            Debug.Log("Te Vi!");
            other.GetComponent<PlayerBehaviour>().Death();
        }
        else
        {
            playerDetected = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }
}
