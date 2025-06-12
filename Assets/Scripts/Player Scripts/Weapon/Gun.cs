using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private float timeSinceLastShot;

    public static Action shootHit;

    private void Awake()
    {
        PlayerBehaviour.shootInput += Shoot;
        weaponData.currentAmmo = weaponData.magSize;
        weaponData.reloading = false;
    }
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }
    private bool CanShoot() => !weaponData.reloading && timeSinceLastShot > 1f / (weaponData.fireRate / 60f);
    public void Shoot()
    {
        if (weaponData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if(this != null)
                {
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, weaponData.maxDistance))
                    {
                        Debug.DrawLine(transform.position, transform.forward * hitInfo.distance, Color.yellow);
                        Debug.Log(hitInfo.transform.name);
                        EnemyAITutorial enemy = hitInfo.transform.GetComponent<EnemyAITutorial>();
                        OnGunShot(enemy);
                    }

                    weaponData.currentAmmo--;
                    timeSinceLastShot = 0f;
                    StartCooldown();
                }
            }
        }
    }
    private void OnGunShot(EnemyAITutorial enemy)
    {
        if (enemy != null)
        {
            enemy.GetHit();
        }
    }
    public void StartCooldown()
    {
        if (!weaponData.reloading)
        {
            StartCoroutine(Cooldown());
        }
    }
    private IEnumerator Cooldown()
    {
        Debug.Log("En enfriamiento...");
        weaponData.reloading = true;
        yield return new WaitForSeconds(weaponData.reloadTime);
        weaponData.reloading = false;
        Debug.Log("Carga Lista!");
    }
}
