using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameStateEnum gameState;
    private int killerNumber;
    private Vector2 positionKiller;
    public int unitsCount=0;

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
                WriteCSV(BoardManager.Instance.gameInfo);
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

    public void WriteCSV(int[] array)
    {
        string ruta = @"D:\Documentos\VJ\TD_Game\TD_GAME\unitsData.csv";
        string separador = ",";
        List<String> filas = new List<string>();
        StringBuilder salida = new StringBuilder();
        string fila = "";
        for (int i = 0; i < array.Length; i++)
        {
            fila += i > 0 ? separador + array[i] : "" + array[i];
            
        }
        filas.Add(fila);
        for (int i = 0; i < filas.Count; i++)
        {
            salida.AppendLine(String.Join(separador, filas[i]));

            File.AppendAllText(ruta, salida.ToString());
        }

    }
}
