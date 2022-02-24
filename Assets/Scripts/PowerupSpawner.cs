using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] float minSpawnDelay = 3f;
    [SerializeField] float maxSpawnDelay = 10f;
    [SerializeField] Powerup powerupPrefab;

    bool spawn = true;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (spawn) {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            SpawnPowerup();
        }
    }

    void SpawnPowerup()
    {
        Vector2 position = new Vector2(Random.Range(-5.5f, 5.5f), transform.position.y);
        Instantiate(powerupPrefab, position, Quaternion.identity);
    }
}
