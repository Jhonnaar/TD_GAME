using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ref: https://drive.google.com/file/d/1WiF2LwM-6WvEnas9vw32YrYPly9K0Qrv/view

public class Player : MonoBehaviour
{
    List<Cell> path;
    [SerializeField]
    private float moveSpeed = 2f;
    public Vector2 GetPosition => transform.position;
    private bool startMoving = false;
    private Grid grid;
    private bool changedCells = false;
    private Rigidbody2D rb;
    private int tipo;
    private string focus;
    public int HP;
    private Collider2D objetive = null;
    private Collider2D objetiveSource = null;
    public int damage;
    //[SerializeField] private GameObject Inner;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;

    public void setFocus(string focus) {
        this.focus = focus;
    }
    public string GetFocus() {
        return this.focus;
    }

    void FixedUpdate()
    {
        //Debug.Log(startMoving);
        if (objetive==null && objetiveSource==null && !startMoving) {
            GetComponent<PlayerShooting>().startShotting = false;
            if (gameObject.tag=="Player")
            {
                starMoving(this.grid, this.moveSpeed);
            }
        }
        if (startMoving)
            Move();
    }

    public void setTypo(int x) {
        this.tipo = x;
    }
    public int getTypo()
    {
        return this.tipo;
    }
    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }


    public void starMoving(Grid grid, float speed)
    {
        this.grid = grid;
        calculatePath();
        startMoving = true;
        moveSpeed = speed;
    }

    private void calculatePath()
    {
        waypointIndex = 0;
        path = PathManager.Instance.FindPath(grid, (int)GetPosition.x, (int)GetPosition.y);
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (focus!="ambos")
        {
            if (collision.gameObject.tag == focus)
            {
                if (collision.gameObject.tag=="PowerSouce")
                {
                    objetiveSource = collision;
                }
                else
                {
                    objetive = collision;
                }
                path = null;
                startMoving = false;
            }
        }
        else
        {
            if (collision.gameObject.tag == "PowerSource" || collision.gameObject.tag == "Tower")
            {
                if (collision.gameObject.tag == "PowerSouce")
                {
                    objetiveSource = collision;
                }
                else
                {
                    objetive = collision;
                }
                path = null;
                startMoving = false;
            }
            else {
                //GetComponent<PlayerShooting>().startShotting = false;
            }
        }
        
        if (collision.gameObject.tag == "Bullet")
        {

            HP -= collision.gameObject.GetComponent<Bullet>().getDamage();
            Destroy(collision.gameObject);
            if (HP < 0)
            {
                /*if (objetive!=null && objetive.gameObject.tag!="PowerSource")
                {
                    Debug.Log(objetive.gameObject.tag);
                    objetive.gameObject.GetComponent<PlayerShooting>().startShotting = false;
                }*/
                if (tag=="Player")
                {
                    GameManager.Instance.unitsCount -= 1;
                }
                Destroy(this.gameObject);
                if (GameManager.Instance.unitsCount<=0)
                {
                    GameManager.Instance.UpdateGameState(GameManager.GameStateEnum.end);
                }
                //GameManager.Instance.UpdateGameState(GameManager.GameStateEnum.end);
            }
        }
        
    }

    private void Move()
    {
        // If player didn't reach last waypoint it can move
        // If player reached last waypoint then it stops
        if (path == null)
            return;

        if (waypointIndex <= path.Count - 1)
        {
            //Debug.Log("Moving to " + path[waypointIndex].transform.position.x.ToString() + " "
            //    + path[waypointIndex].transform.position.y.ToString());

            if (changedCells) {
                changedCells = false;
                if (!grid.isWalkable((int)path[waypointIndex].transform.position.x, (int)path[waypointIndex].transform.position.y))
                {
                    //Debug.Log("not walkable");
                    //path = null;
                    calculatePath();
                    return;
                } else
                {
                    grid.setBusyCell((int)path[waypointIndex - 1].transform.position.x,
                        (int)path[waypointIndex - 1].transform.position.y,
                        (int)path[waypointIndex].transform.position.x,
                        (int)path[waypointIndex].transform.position.y);
                }
                
            }
            // Move player from current waypoint to the next one
            // using MoveTowards method
            transform.position = Vector2.MoveTowards(transform.position,
               path[waypointIndex].transform.position,
               moveSpeed * Time.deltaTime);

            // If player reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and player starts to walk to the next waypoint
            if (transform.position == path[waypointIndex].transform.position)
            {
                waypointIndex += 1;
                changedCells = true;
            }
        }
    }
    public void setHp(int HP) {
        this.HP = HP;
    }
    public int getHp() {
        return HP;
    }
}
