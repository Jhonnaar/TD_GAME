using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour
{
    public int HP = 140;

    private void Awake()
    {
        HP = 140;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            
            HP -= collision.gameObject.GetComponent<Bullet>().getDamage();
            Destroy(collision.gameObject);
            if (HP < 0)
            {
                Destroy(this.gameObject);
                BoardManager.Instance.gameData[BoardManager.Instance.gameData.Length-1] = 1;
                Data.Instance.SetGame(true);
                GameManager.Instance.UpdateGameState(GameManager.GameStateEnum.end);
            }
        }
    }
}
