using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public float speedX;
    public float JumpHeight;
    public Color hurtColor = Color.red;
    public Color normalColor = Color.white;

    bool facingRight;

    public GameObject leftBullet;
    private bool hasKey;
    public GameObject rightBullet;
    Player player;
    bool isGrounded;
    bool hasLeftGround;
    float speed;
    bool cantBeHurt;


    private bool wasRunningBeforeJump = false;
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
        facingRight = true;
        player = GetComponent<Player>();
        healthText.text = "Health: " + player.playerStats.health.ToString();
        hasKey = false;
        firePoint = transform.FindChild("firePoint");
    }

    void Update()
    {
        GroundCheck();
        MovePlayer(speed);
        // player movement

        // left player movement
        if (Input.GetKeyDown(KeyCode.LeftArrow) && isGrounded)
            speed = -speedX;
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            StopWalk();
        //

        // right player movement
        if (Input.GetKeyDown(KeyCode.RightArrow) && isGrounded)
            speed = speedX;
        if (Input.GetKeyUp(KeyCode.RightArrow))
            StopWalk();
        //

        Flip();

        if (rb.velocity == Vector2.zero)
        {
            IdleAnimation();
        }

        // jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            Jump();

        }

        // shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }

    }

    void MovePlayer(float playerSpeed)
    {
        // code for player movement
        if (playerSpeed < 0 && isGrounded || playerSpeed > 0 && isGrounded)
        {
            RunAnimation();
        }
        if (playerSpeed == 0 && isGrounded)
        {
            IdleAnimation();
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
        if (facingRight)
            Instantiate(rightBullet, firePoint.position, Quaternion.identity);
        if (!facingRight)
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
