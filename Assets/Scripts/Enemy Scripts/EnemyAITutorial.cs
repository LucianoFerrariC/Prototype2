using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyAITutorial : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint;
    public float speed;
    public float turnSpeed = 360f;
    public LightDetection lightDetection;

    [SerializeField] private Light spotlight;
    [SerializeField] private CapsuleCollider detectionTrigger;

    [SerializeField] private AudioClip droneFlying;
    private AudioSource audioSource;

    private void Start()
    {
        targetPoint = 0;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        Patroling();
        if (lightDetection.playerDetected == true)
        {
            AttackPlayer();
        }
    }
    private void Patroling()
    {
        if (transform.position == patrolPoints[targetPoint].position)
        {
            targetPoint++;
            if (targetPoint >= patrolPoints.Length)
            {
                targetPoint = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);
        var rotation = Quaternion.LookRotation(patrolPoints[targetPoint].forward, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }
    private void AttackPlayer()
    {

    }
    public void GetHit()
    {
        StartCoroutine(ShockHit());
    }
    private IEnumerator ShockHit()
    {
        speed = 0;
        spotlight.color = Color.green;
        detectionTrigger.enabled = false;
        yield return new WaitForSeconds(5f);
        speed = 4;
        spotlight.color = Color.yellow;
        detectionTrigger.enabled = true;
    }

    //Sounds
    public void FlyingSound()
    {
        audioSource.pitch = Random.Range(0.8f,1f);
        audioSource.PlayOneShot(droneFlying);
    }
}
