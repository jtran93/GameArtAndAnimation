using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public float speed;
    public float JumpHeight;
    public Color hurtColor = Color.red;
    public Color normalColor = Color.white;

    public bool FacingRight
    {
        get { return spriteRenderer.flipX == false; }
        set { spriteRenderer.flipX = !value; }
    }

    public GameObject leftBullet;
    private bool hasKey;
    public GameObject rightBullet;
    Player player;
    bool isGrounded;
    bool hasLeftGround;
    bool cantBeHurt;
    bool isMoveRight;
    bool isMoveLeft;

    private SpriteRenderer spriteRenderer;
    private Renderer renderer;
    private Animator animator;
    public Text healthText;
    private BoxCollider2D boxCollider;
    private Transform firePoint;
    private Rigidbody2D rb;
    

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        renderer = GetComponent<Renderer>();
        cantBeHurt = false;
        player = new Player();
        healthText.text = "Health: " + player.playerStats.health.ToString();
        hasKey = false;
        firePoint = transform.FindChild("firePoint");
    }

    void Update()
    {
        GroundCheck();
        // player movement

        // Check to see if there even is an input
        if (isMoveRight && isGrounded)
        {
            Move(1);
            RunAnimation();
        }

        else if (isMoveLeft && isGrounded)
        {
            Move(-1);
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

    void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.gameObject.tag == "Enemy" && !cantBeHurt)
        {
            player.damagePlayer(1);
            setHealthText();
            StartCoroutine(Flasher());
            if(player.playerStats.health <= 0)
            {
                Destroy(gameObject);
                loadScene("Fail");
            }

        }
        else if (otherObject.gameObject.tag == "Key")
        {
            hasKey = true;
            Destroy(otherObject.gameObject);
        }
        else if(otherObject.gameObject.tag == "Door")
        {
            Debug.Log("Door Touched");
            Debug.Log(hasKey);
            if (hasKey)
                loadScene("Victory");
        }
    }

    void OnTriggerStay2D(Collider2D otherObject)
    {
        if (otherObject.gameObject.tag == "Enemy" && !cantBeHurt)
        {
            player.damagePlayer(1);
            setHealthText();
            StartCoroutine(Flasher());
            if (player.playerStats.health <= 0)
            {
                Destroy(gameObject);
                loadScene("Fail");
            }

        }
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
        //jumping = true;
        rb.AddForce(new Vector2(rb.velocity.x, JumpHeight));
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

    private void RunAnimation()
    {
        animator.SetInteger("State", 2);
    }

    private void IdleAnimation()
    {
        animator.SetInteger("State", 0);
    }

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void setHealthText()
    {
        healthText.text = "Health: " + player.playerStats.health.ToString();
    }

    IEnumerator Flasher()
    {
        for (int i = 0; i < 5; i++)
        {
            renderer.material.color = hurtColor;
            cantBeHurt = true;
            yield return new WaitForSeconds(.1f);
            renderer.material.color = normalColor;
            yield return new WaitForSeconds(.1f);
            cantBeHurt = false;
        }
    }

    public void OnPointerDownRight()
    {
        isMoveRight = true;
    }

    public void OnPointerUpRight()
    {
        isMoveRight = false;
        rb.velocity = Vector2.zero;
        
    }

    public void OnPointerDownLeft()
    {
        isMoveLeft = true;
        
    }

    public void OnPointerUpLeft()
    {
        isMoveLeft = false;
        rb.velocity = Vector2.zero;
    }
}
