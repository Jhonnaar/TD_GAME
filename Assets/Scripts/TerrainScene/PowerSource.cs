using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour
{
    public int HP = 1400;

    private void Awake()
    {
        HP = 1400;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit");
        if (collision.gameObject.tag == "Bullet")
        {
            
            HP -= collision.gameObject.GetComponent<Bullet>().getDamage();
            Debug.Log("Hit by a bullet, new HP "+ HP);
            Destroy(collision.gameObject);
            if (HP < 0)
            {
                Destroy(this.gameObject);
                GameManager.Instance.UpdateGameState(GameManager.GameStateEnum.end);
            }
        }
    }
}
