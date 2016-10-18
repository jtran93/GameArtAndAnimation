using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerManager : MonoBehaviour
{

    public float speedX;
    public float jumpSpeedY;
    public GameObject leftBullet;
    public GameObject rightBullet;

    bool isGrounded;
    bool hasLeftGround;
    bool facingRight;
    float speed;
    Animator animator;
    BoxCollider2D boxCollider;
    Transform firePoint;
    Rigidbody2D rb;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;
        firePoint = transform.FindChild("firePoint");
    }

    void Update()
    {

        GroundCheck(); 
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
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
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
        if (playerSpeed < 0 && isGrounded || playerSpeed > 0 && isGrounded)
        {
            animator.SetInteger("State", 2);
        }
        if (playerSpeed == 0 && isGrounded)
        {
            animator.SetInteger("State", 0);
        }
        rb.velocity = new Vector3(speed, rb.velocity.y, 0);
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
        if (facingRight)
            Instantiate(rightBullet, firePoint.position, Quaternion.identity);
        if (!facingRight)
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

    private void GroundCheck()
    {
        // Cast a box below us just a hair to see if there's any objects below us
        var hits = Physics2D.BoxCastAll(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x * 0.9f, boxCollider.bounds.size.y), 0.0f, Vector2.down, 0.1f);

        // Check to see if any of the things we hit is in the Terrain layer and that we've already left the ground
        if (hits.Any(hit => hit.collider.gameObject.layer == LayerMask.NameToLayer("GROUND")) && hasLeftGround)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;

            // If we can't hit the ground anymore, that means we've successfully left the ground below us
            hasLeftGround = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);

        }
    }
}
