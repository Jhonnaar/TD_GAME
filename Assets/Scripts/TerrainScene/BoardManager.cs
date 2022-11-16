using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEditorInternal;
using ClipperLib;
using System.Net.Http;
//using Newtonsoft.Json;

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
    public int[] gameInfo = new int[67];
    private string url = "https://td-game-api.herokuapp.com/positions"; 
    
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
        GenUnitsIA();
        genTorres(presupuestoTorres);
        //genUnidades(presupuesto);

        //player = Instantiate(PlayerPrefab, new Vector2(0, 0), Quaternion.identity);

        //player.starMoving(grid, 2);

        //player = Instantiate(PlayerPrefab, new Vector2(8, 0), Quaternion.identity);

        //player.starMoving(grid, 3);
    }

    public async void GenUnitsIA()
    {
        try
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                result = result.Split('[')[1];
                result = result.Split(']')[0];
                var lista = result.Split(',');
                for (int i = 0; i < lista.Length; i++)
                {
                    int item = System.Int32.Parse(lista[i]);
                    if (item != 0)
                    {
                        int posX = i;
                        int posY = (i >= 11) ? (i / 11) : 0;
                        for (int j = 0; j < posY; j++)
                        {
                            posX -= 11;
                        }
                        //Debug.Log("x: " + posX + ", Y: " + posY);
                        player = Instantiate(PlayerPrefab, new Vector2(posX, posY), Quaternion.identity);
                        player.tag = "Player";
                        //Debug.Log("Pocision: " + player.transform.position);
                        //Debug.Log("Celda: " + UnitCell((int)player.transform.position.x+1, (int)player.transform.position.y+1));
                        int celda = UnitCell((int)player.transform.position.x + 1, (int)player.transform.position.y + 1);
                        player.gameObject.layer = 9;
                        player.setTypo(item);
                        GameManager.Instance.unitsCount += 1;
                        gameInfo[celda - 1] = player.getTypo();
                        switch (item)
                        {
                            case 1:
                                player.SetColor(Color.blue);
                                player.GetComponent<CircleCollider2D>().radius = 3;
                                player.setFocus("ambos");
                                player.damage = 10;
                                player.HP = 40;
                                break;
                            case 2:
                                player.SetColor(Color.black);
                                player.GetComponent<CircleCollider2D>().radius = 3;
                                player.setFocus("ambos");
                                player.damage = 20;
                                player.HP = 60;
                                break;
                            case 3:
                                player.SetColor(Color.green);
                                player.GetComponent<CircleCollider2D>().radius = 2;
                                player.setFocus("PowerSource");
                                player.damage = 10;
                                player.HP = 80;
                                break;
                        }
                        player.starMoving(grid, item == 2 ? 2 : 4);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("error "+e);
        }
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
        //Debug.Log("presupuesto restante: "+(cont-n));
        foreach (var item in unidades)
        {

            player = Instantiate(PlayerPrefab, new Vector2(Random.Range(0, 11), Random.Range(0, 6)), Quaternion.identity);
            player.tag = "Player";
            //Debug.Log("Pocision: " + player.transform.position);
            //Debug.Log("Celda: " + UnitCell((int)player.transform.position.x+1, (int)player.transform.position.y+1));
            int celda = UnitCell((int)player.transform.position.x + 1, (int)player.transform.position.y + 1);
            player.gameObject.layer = 9;
            player.setTypo(item);
            GameManager.Instance.unitsCount += 1;
            gameInfo[celda-1] = player.getTypo();
            switch (item)
            {
                case 1:
                    player.SetColor(Color.blue);
                    player.GetComponent<CircleCollider2D>().radius = 3;
                    player.setFocus("ambos");
                    player.damage = 10;
                    player.HP = 40;
                    break;
                case 2:
                    player.SetColor(Color.black);
                    player.GetComponent<CircleCollider2D>().radius = 3;
                    player.setFocus("ambos");
                    player.damage = 20;
                    player.HP = 60;
                    break;
                case 3:
                    player.SetColor(Color.green);
                    player.GetComponent<CircleCollider2D>().radius = 2;
                    player.setFocus("PowerSource");
                    player.damage = 10;
                    player.HP = 80;
                    break;
            }
            player.starMoving(grid, item==2? 2:4);
        }

    }
    public int UnitCell(int posX, int posY) 
    {
        int result=0;
        for (int i = 1; i < posY; i++)
        {
            result += 11;
        }
        result += posX;
        return result;
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
        //Debug.Log("presupuesto restante: " + (cont - n));
        foreach (var item in unidades)
        {
            player = Instantiate(PlayerPrefab, new Vector2(Random.Range(0, 11), Random.Range(13, 19)), Quaternion.identity);
            player.tag = "Tower";
            player.gameObject.layer = 10;
            player.setTypo(item);
            player.setFocus("Player");
            switch (item)
            {
                case 2:
                    player.SetColor(Color.red);
                    player.GetComponent<CircleCollider2D>().radius = 3;
                    player.damage = 20;
                    player.HP = 20;
                    break;
                case 3:
                    player.SetColor(Color.cyan);
                    player.GetComponent<CircleCollider2D>().radius = 3;
                    player.damage = 30;
                    player.HP = 40;
                    break;
            }
            //player.starMoving(grid, item == 2 ? 2 : 4);
        }

    }
}
