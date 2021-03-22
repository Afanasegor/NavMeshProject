using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] private float bulletVelocity = 5f;
    [SerializeField] private ParticleSystem fire;    

    public void Shoot()
    {
        fire.Play();
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        newBullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletVelocity;
    }
}