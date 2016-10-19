using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerManager : MonoBehaviour
{




    public float speed;
    public float JumpHeight;

    public bool FacingRight
    {
        get { return spriteRenderer.flipX == false; }
        set { spriteRenderer.flipX = !value; }
    }
    public GameObject leftBullet;
    public GameObject rightBullet;

    bool isGrounded;
    bool hasLeftGround;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private Transform firePoint;
    private Rigidbody2D rb;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        firePoint = transform.FindChild("firePoint");
    }

    void Update()
    {

        GroundCheck();
        var horizontalInput = Input.GetAxis("Horizontal");
        // player movement

        // Check to see if there even is an input
        if (Mathf.Abs(horizontalInput) > 0.0f && isGrounded)
        {
            Move(horizontalInput);
            RunAnimation();
        }

        else if (Mathf.Abs(horizontalInput) < 0.0f && isGrounded)
        {
            Move(horizontalInput);
            RunAnimation();
        }


        // There was no input, so we should be idle
        else if (rb.velocity == Vector2.zero)
        {
            IdleAnimation();
        }

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

    public void Move(float direction)
    {
        // If our direction is positive, we're moving to the right
        if (direction > 0.0f)
        {
            FacingRight = true;
            // Otherwise, we're going left
            // Note: we don't care about 0.0f, because it'd be unusual for our character to constantly face right
        }
        else if (direction < 0.0f)
        {
            FacingRight = false;
        }

        // Set our Velocity to move on the next FixedUpdate tick
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }


    public void Fire()
    {
        if (FacingRight)
            Instantiate(rightBullet, firePoint.position, Quaternion.identity);
        if (!FacingRight)
            Instantiate(leftBullet, firePoint.position, Quaternion.identity);
    }

    public void Jump()
    {
        rb.AddForce(new Vector2(rb.velocity.x, Mathf.Sqrt(-2.0f * JumpHeight * Physics2D.gravity.y)));
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

    private void RunAnimation()
    {
        animator.SetInteger("State", 2);
    }

    private void IdleAnimation()
    {
        animator.SetInteger("State", 0);
    }
}
