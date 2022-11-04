using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameStateEnum gameState;
    private int killerNumber;
    private Vector2 positionKiller;

    private void Awake()
    {
        Instance = this;
        UpdateGameState(GameStateEnum.start);
    }

    public void UpdateGameState(GameStateEnum gameStateEnum)
    {
        gameState = gameStateEnum;
    }

    void Update()
    {
        switch (gameState)
        {
            case GameStateEnum.start:

                BoardManager.Instance.SetupBoard();
                UpdateGameState(GameStateEnum.progress);
                break;
            case GameStateEnum.progress:
                break;
            case GameStateEnum.end:
                SceneManager.LoadScene("TerrainScene");
                break;
        }

    }

    public enum GameStateEnum
    {
        start,
        progress, 
        end
    }
    public void setKillerNumber(string n) {
        killerNumber = Int32.Parse(n);
    }
    public void setPositionKiller(string pos) {
        positionKiller = new Vector2(float.Parse(pos.Split(',')[0]), float.Parse(pos.Split(',')[1]));
    }
    public void genUnidades(int n) {
        
    }
}
