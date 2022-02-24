using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    int type;
    int numOfAttrs = 6;

    [Header("Sprites")]
    [SerializeField] Sprite healthSprite;
    [SerializeField] Sprite increaseFireSpeedSprite;
    [SerializeField] Sprite instaKillSprite;
    [SerializeField] Sprite slowEnemiesSprite;
    [SerializeField] Sprite speedUpSelfSprite;

    Coroutine speedupCoroutine;
    Coroutine fireRateCoroutine;

    // Start is called before the first frame update
    void Start() { SetUpPowerup(); }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player) { HandlePowerupEffect(player); }
    }

    // TODO: do insta-kill, slow enemies powerups
    void HandlePowerupEffect(Player player)
    {
        // health powerup
        if (type == 1) { player.ReceiveHealth(); }
        // increase fire rate powerup
        else if (type == 2) {
            Debug.Log("Starting increase fire rate");
            player.IncreaseFireRate();
            StartCoroutine(player.StopIncreaseFireRate());
            print("after coroutine in health powerup");
        }
        // speed up player powerup
        else if (type == 5) {
            player.SpeedUp();
            StartCoroutine(player.StopSpeedUp());
            print("after coroutine in speed up player powerup");
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 10);
    }

    void SetUpPowerup()
    {
        // determine type, set sprite
        type = Random.Range(1, numOfAttrs);

        switch (type)
        {
            // health
            case 1:
                GetComponent<SpriteRenderer>().sprite = healthSprite;
                break;
            // increase fire speed 
            case 2:
                print("increase fire speed powerup");
                GetComponent<SpriteRenderer>().sprite = increaseFireSpeedSprite;
                break;

            // insta kill
            // case 3:
            //     print("insta-kill powerup");
            //     GetComponent<SpriteRenderer>().sprite = instaKillSprite;
            //     break;
            // // slow enemies
            // case 4:
            //     print("slow enemies powerup");
            //     GetComponent<SpriteRenderer>().sprite = slowEnemiesSprite;
            //     break;

            // // speed up player 
            // case 5:
            //     GetComponent<SpriteRenderer>().sprite = speedUpSelfSprite;
            //     break;
            default:
                break;
        }
    }
}
