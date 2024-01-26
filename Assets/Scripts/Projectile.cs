using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public PlayerScript Ps;
    public Rigidbody2D rb;
    public float speed;
    public Vector2 StartPos;

    public float DistanceToDestroy;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
        Ps = FindObjectOfType<PlayerScript>();
        StartPos = Ps.gameObject.transform.position - transform.position;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().UpdateHealth(-50);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Map")
        {
            Ps.rb.AddForce(StartPos * Ps.JumpForce, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void FixedUpdate()
    {
        if(Vector2.Distance(transform.position,Ps.transform.position) >= DistanceToDestroy)
        {
            Destroy();
        }
    }
}
