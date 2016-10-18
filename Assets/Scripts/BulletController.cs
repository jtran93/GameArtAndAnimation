using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    public Vector2 speed;
    Rigidbody2D rb;
    
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;	
	}
	
	void Update ()
    {
        rb.velocity = speed;
	}

    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if(otherObject.gameObject.tag != "Player")
            Destroy(gameObject);
    }
}
