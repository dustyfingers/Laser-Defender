using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.25f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float explosionDuration = 0.5f;
    [SerializeField] float projectileSpeed = -4f;

    [Header("Sound Effects")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.25f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.15f;

    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    void Update()
    {
        CountDownAndShoot();
    }

    void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    void Fire()
    {
        // instantiate laser prefab as game object
        GameObject laser = Instantiate(
            laserPrefab,
            transform.position,
            Quaternion.identity
        ) as GameObject;

        Vector2 laserVelocity = new Vector2(0, projectileSpeed);

        laser.GetComponent<Rigidbody2D>().velocity = laserVelocity;

        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

    void OnTriggerEnter2D(Collider2D otherThing)
    {
        DamageDealer damageDealer = otherThing.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    void ProcessHit(DamageDealer damageDealer)
    {

        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // find the gamesession obj and call its addtoscore method to 
        // increment the score with the proper scorevalue
        FindObjectOfType<GameSession>().AddToScore(scoreValue);

        Destroy(gameObject);

        GameObject explosion = Instantiate(
            explosionPrefab,
            transform.position,
            transform.rotation
        ) as GameObject;

        Destroy(explosion, explosionDuration);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}
