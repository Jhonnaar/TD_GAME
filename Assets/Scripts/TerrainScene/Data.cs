using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data Instance;
    private static int totalGames = 0;
    private static int winGames = 0;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        //Data.DontDestroyOnLoad(gameObject);
    }

    public void FixedUpdate()
    {
        GameManager.Instance.winRate.text = "Score: " + ((float)winGames / (float)totalGames) * 100 + "%";
    }

    public void SetGame(bool won) {
        if (won)
        {
            winGames++;
            totalGames++;
        }
        else
        {
            totalGames++;
        }
    }
}
