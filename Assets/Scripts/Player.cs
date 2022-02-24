using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 400;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.25f;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] [Range(0, 1)] float gameOverSoundVolume = 0.35f;
    [SerializeField] [Range(1, 5)] float speedUpAmount = 1.75f;
    [SerializeField] [Range(1, 5)] float increaseFireRateAmount = 2f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 4f;
    [SerializeField] float projectileFiringPeriod = 0.15f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.15f;

    // coroutine state vars
    Coroutine firingCoroutine;

    // min and max boundaries for player movement 
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // speed coefficient for changing speed with powerup
    float speedCoeff = 1f;
    float fireRateCoeff = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    // handles player firing 
    void Fire()
    {
        if (Input.GetButtonDown("Fire1")) { firingCoroutine = StartCoroutine(FireContinuously()); }
        if (Input.GetButtonUp("Fire1")) { StopCoroutine(firingCoroutine); }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            // instantiate laser prefab as game object
            GameObject laser = Instantiate(
                laserPrefab,
                transform.position,
                laserPrefab.transform.rotation
            ) as GameObject;

            Vector2 laserVelocity = new Vector2(0, projectileSpeed);

            laser.GetComponent<Rigidbody2D>().velocity = laserVelocity;
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);

            yield return new WaitForSeconds(projectileFiringPeriod * (1 / fireRateCoeff));
        }
    }

    // handles all player movement
    void Move()
    {
        // calculate new x and y position and build new position vector
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed * speedCoeff;
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed * speedCoeff;

        float newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        float newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        Vector2 newPos = new Vector2(newXPos, newYPos);

        // assign new position to transform position
        transform.position = newPos;
    }

    // sets up movement boundaries for player using screen dimension
    void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    void OnTriggerEnter2D(Collider2D otherThing)
    {
        DamageDealer damageDealer = otherThing.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    void Die()
    {
        // destroy self and play death sound
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);

        // after brief pause play game over sound and load game over scene
        AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position, gameOverSoundVolume);

        // how to access public methods on other classes
        FindObjectOfType<Level>().LoadGameOver();
    }

    void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (health <= 0) { Die(); }
    }

    public int GetHealth() { return health; }

    public void SpeedUp() { speedCoeff = speedUpAmount; }

    public IEnumerator StopSpeedUp()
    {
        print("start of Player.StopSpeedUp()");
        yield return new WaitForSeconds(5);
        print("waited 5 seconds, end on Player.StopSpeedUp()");
        speedCoeff = 1f;
    }

    public void ReceiveHealth() { print("giving health"); health += 100; }

    public void IncreaseFireRate() { fireRateCoeff = increaseFireRateAmount; }

    public IEnumerator StopIncreaseFireRate() 
    {
        print("stopping increased fire rate");
        yield return new WaitForSeconds(5);
        print("waited 5 seconds");
        fireRateCoeff = 1f;
    }
}
