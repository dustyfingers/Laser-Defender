using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;

    void Awake()
    {
        SetUpSingleton();
    }

    void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public int AddToScore(int scoreValue)
    {
        return score += scoreValue;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
