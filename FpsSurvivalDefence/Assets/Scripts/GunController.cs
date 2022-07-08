using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }
    
    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // 60분의 1
        }
    }
    
    private void TryFire()
    {
        if (Input.GetButton("Fire1")&& currentFireRate <=0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        PlaySE(currentGun.fire_Sound);
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    private void Shoot()
    {
        currentGun.muzzleFlash.Play();
        Debug.Log("총알 발사함");
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
