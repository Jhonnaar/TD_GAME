using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public Bullet bulletPrefab;
    public int bulletForce;
    public float shootingInterval = 0.3f;
    private float period = 0.0f;
    public bool startShotting = false;
    private Rigidbody2D rb;
    private Vector2 targetPosicion;
    private Collider2D target;

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }
    
    //https://www.youtube.com/watch?v=LNLVOjbrQj4

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string focus = GetComponent<Player>().GetFocus();
        if (focus != "ambos")
        {
            if (collision.gameObject.tag == focus)
            {
                startShotting = true;
                bulletForce = GetComponent<Player>().damage;
                targetPosicion = collision.gameObject.transform.position;
                target = collision;
            }
        }
        else
        {
            if (collision.gameObject.tag == "PowerSource" || collision.gameObject.tag == "Tower")
            {
                startShotting = true;
                bulletForce = GetComponent<Player>().damage;
                targetPosicion = collision.gameObject.transform.position;
                target = collision;
            }
        }
    }

    public void lookAtTarget()
    {
        Vector2 lookDir = targetPosicion - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        rb.rotation = angle;
    }

    void  FixedUpdate()
    {
        if (targetPosicion!=null && target!=null)
        {
            
            targetPosicion = target.gameObject.transform.position;
            lookAtTarget();
        }
        if (!startShotting)
            return;

        if (period > shootingInterval)
        {
            shoot();
            period = 0;
        }
        period += UnityEngine.Time.deltaTime;
    }

    private void shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (gameObject.layer == 9)
        {
            bullet.gameObject.layer = 8;
        }
        else {
            bullet.gameObject.layer = 7;
        }
        bullet.setDamage(bulletForce);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
