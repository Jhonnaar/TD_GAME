using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    [SerializeField] private Cell CellPrefab;
    [SerializeField] private Player PlayerPrefab;
    [SerializeField] private PowerSource PowerSourcePrefab;
    private Grid grid;
    private Player player;
    private int presupuesto = 15;
    private int presupuestoTorres = 15;
    [SerializeField]
    //private float moveSpeed = 2f;

    private void Awake()
    {
        Instance = this;
    }

    public void SetupBoard()
    {
        grid = new Grid(11, 20, 1, CellPrefab);

        Instantiate(PowerSourcePrefab, new Vector2(5, 19), Quaternion.identity);

        PathManager.Instance.powerUnitLocation = new Vector2Int(5, 19);

        genTorres(presupuestoTorres);
        genUnidades(presupuesto);

        //player = Instantiate(PlayerPrefab, new Vector2(0, 0), Quaternion.identity);

        //player.starMoving(grid, 2);

        //player = Instantiate(PlayerPrefab, new Vector2(8, 0), Quaternion.identity);

        //player.starMoving(grid, 3);
    }
    public void genUnidades(int n)
    {
        List<int> unidades = new List<int>();
        int cont = 0;
        while (n != cont)
        {
            if (cont < n)
            {
                int x = Random.Range(1, 4);
                unidades.Add(x);
                cont += x;
            }
            else
            {
                unidades = new List<int>();
                cont = 0;
            }
        }
        Debug.Log("presupuesto restante: "+(cont-n));
        foreach (var item in unidades)
        {
            player = Instantiate(PlayerPrefab, new Vector2(Random.Range(0, 11), Random.Range(0, 6)), Quaternion.identity);
            player.tag = "Player";
            player.setTypo(item);
            switch (item)
            {
                case 1:
                    player.SetColor(Color.blue);
                    player.GetComponent<CircleCollider2D>().radius = 3;
                    player.setFocus("ambos");
                    break;
                case 2:
                    player.SetColor(Color.black);
                    player.GetComponent<CircleCollider2D>().radius = 3;
                    player.setFocus("ambos");
                    break;
                case 3:
                    player.SetColor(Color.green);
                    player.GetComponent<CircleCollider2D>().radius = 2;
                    player.setFocus("PowerSource");
                    break;
            }
            player.starMoving(grid, item==2? 2:4);
        }

    }
    public void genTorres(int n)
    {
        List<int> unidades = new List<int>();
        int cont = 0;
        while (n != cont)
        {
            if (cont < n)
            {
                int x = Random.Range(2, 4);
                unidades.Add(x);
                cont += x;
            }
            else
            {
                unidades = new List<int>();
                cont = 0;
            }
        }
        Debug.Log("presupuesto restante: " + (cont - n));
        foreach (var item in unidades)
        {
            player = Instantiate(PlayerPrefab, new Vector2(Random.Range(0, 11), Random.Range(13, 19)), Quaternion.identity);
            player.tag = "Tower";
            player.setTypo(item);
            player.setFocus("Player");
            switch (item)
            {
                case 2:
                    player.SetColor(Color.red);
                    player.GetComponent<CircleCollider2D>().radius = 3;
                    break;
                case 3:
                    player.SetColor(Color.cyan);
                    player.GetComponent<CircleCollider2D>().radius = 3;
                    break;
            }
            //player.starMoving(grid, item == 2 ? 2 : 4);
        }

    }
}
