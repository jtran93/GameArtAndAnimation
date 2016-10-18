using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{

    public float speedX;
    public float jumpSpeedY;
    public GameObject leftBullet;
    public GameObject rightBullet;

    bool facingRight;
    bool jumping;
    float speed;
    Animator animator;
    Transform firePoint;
    Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;
        firePoint = transform.FindChild("firePoint");
    }

    void Update()
    {
        // player movement
        MovePlayer(speed);

        // left player movement
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            speed = -speedX;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            StopWalk();
        }
        //

        // right player movement
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            speed = speedX;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            StopWalk();
        }
        //

        Flip();

        // jump
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        //

        // shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        //

    }

    void MovePlayer(float playerSpeed)
    {
        // code for player movement
        if(playerSpeed < 0 && !jumping || playerSpeed > 0 && !jumping)
        {
            animator.SetInteger("State", 2);
        }
        if(playerSpeed == 0 && !jumping)
        {
            animator.SetInteger("State", 0);
        }
        rb.velocity = new Vector3(speed, rb.velocity.y, 0);
    }

    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if(otherObject.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
    

    void Flip()
    {
        if (speed > 0 && !facingRight || speed < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 temp = transform.localScale;
            temp.x *= -1;
            transform.localScale = temp;
        }
    }

    public void Fire()
    {
        if(facingRight)
            Instantiate(rightBullet, firePoint.position, Quaternion.identity);
        if(!facingRight)
            Instantiate(leftBullet, firePoint.position, Quaternion.identity);
    }

    public void Jump()
    {
        //jumping = true;
        rb.AddForce(new Vector2(rb.velocity.x, jumpSpeedY));
    }

    public void WalkLeft()
    {
        speed = -speedX;
    }

    public void WalkRight()
    {
        speed = speedX;
    }

    public void StopWalk()
    {
        speed = 0;
    }
}
