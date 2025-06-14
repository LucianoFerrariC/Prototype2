using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyAITutorial : MonoBehaviour
{
    [Header("Variables")]

    [Range(0.0f, 10.0f)] public float speed;
    [Range(0.0f, 10.0f)] public float secondsDisabled;
    private float turnSpeed = 360f;
    private int targetPoint;

    [Header("External Components")]
    [SerializeField] private LightDetection lightDetection;
    [SerializeField] private Light spotlight;
    [SerializeField] private CapsuleCollider detectionTrigger;
    public Transform[] patrolPoints;

    [Header("List of Sounds")]
    [SerializeField] private AudioClip onPatrolling;
    [SerializeField] private AudioClip onDisabled;
    [SerializeField] private AudioClip onAttacking;

    private AudioSource audioSource;

    private void Start()
    {
        targetPoint = 0;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(patrolPoints.Length != 0)
        {
            Patroling();
        }
    }
    private void Patroling()
    {
        if (transform.position == patrolPoints[targetPoint].position)
        {
            targetPoint++;
            audioSource.PlayOneShot(onPatrolling);
            if (targetPoint >= patrolPoints.Length)
            {
                targetPoint = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);
        var rotation = Quaternion.LookRotation(patrolPoints[targetPoint].forward, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }
    public void GetHit()
    {
        StartCoroutine(ShockHit());
    }
    private IEnumerator ShockHit()
    {
        audioSource.PlayOneShot(onDisabled);
        speed = 0;
        spotlight.color = Color.green;
        detectionTrigger.enabled = false;
        yield return new WaitForSeconds(secondsDisabled);
        speed = 4;
        spotlight.color = Color.red;
        detectionTrigger.enabled = true;
    }

    //Sounds
    public void PatrollingSound()
    {
        audioSource.PlayOneShot(onPatrolling);
    }
}
