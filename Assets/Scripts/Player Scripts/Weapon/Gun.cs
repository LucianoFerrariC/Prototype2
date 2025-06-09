using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private float timeSinceLastShot;

    private void Start()
    {
        PlayerBehaviour.shootInput += Shoot;
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
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, weaponData.maxDistance))
                {
                    Debug.Log(hitInfo.transform.name);
                }
                weaponData.currentAmmo--;
                timeSinceLastShot = 0f;
                StartCooldown();
                OnGunShot();
            }
        }
    }
    private void OnGunShot()
    {

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
