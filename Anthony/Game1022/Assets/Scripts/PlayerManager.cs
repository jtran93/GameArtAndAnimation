using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public Color hurtColor = Color.red;
    public Color normalColor = Color.white;

    private ActorController controller;
    private Jumping jumper;

    public bool touchScreenMode;                                //if you want to use WASD, make it false. if you want to use buttons, true.
    public GameObject leftBullet;
    private bool hasKey = false;
    public GameObject rightBullet;
    Player player;
    float speed;
    bool cantBeHurt = false;
    bool facingRight = true;
    private bool moveLeft = false;
    private bool moveRight = false;
    private bool jumping = false;
    private bool runAnimation = false;
    
    private bool wasRunningBeforeJump = false;
    private Renderer renderer;
    private Animator animator;
    public Text healthText;
    private Transform firePoint;
    private Rigidbody2D rb;
    

    public void Awake()
    {
        controller = GetComponent<ActorController>();
        jumper = GetComponent<Jumping>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();
        player = GetComponent<Player>();
        healthText.text = "Health: " + player.playerStats.health.ToString();
        firePoint = transform.FindChild("firePoint");
    }

    void Update()
    {

        var horizontalInput = Input.GetAxis("Horizontal");
        // player movement

        // left player movement
        if (Mathf.Abs(horizontalInput) > 0.0f || moveRight || moveLeft)                 //check for keyboard movement, or check if eventTriggers were pressed.
            {         
                

                if (horizontalInput > 0.0f || moveRight == true)                        //if Right eventTrigger pressed, or if player trying to move right
                {
                    if(moveRight == true)
                        horizontalInput = 1;
                    if (controller.IsGrounded || wasRunningBeforeJump)
                    {
                        wasRunningBeforeJump = true;
                        controller.Move(horizontalInput);
                    }
                    else
                        controller.Move(horizontalInput * .5f);
                    facingRight = true;
                }

                if (horizontalInput < 0.0f || moveLeft == true)                         //if Left event trigger pressed, or if player trying to move left
                {
                    if(moveLeft == true)
                        horizontalInput = -1;
                    if (controller.IsGrounded || wasRunningBeforeJump)
                    {
                        wasRunningBeforeJump = true;
                        controller.Move(horizontalInput);
                    }
                    else
                        controller.Move(horizontalInput * .5f);
                    facingRight = false;
                }

                if (controller.IsGrounded)                                              //no running animation unless player grounded.
                    RunAnimation();
        }

        //idle
        if (controller.Velocity == Vector2.zero && !touchScreenMode)                                       //if player isn't moving and touchScreenMode isnt active.
        {
            IdleAnimation();
        }
        if(!moveLeft && !moveRight && touchScreenMode && controller.IsGrounded)                             
        {
            rb.velocity = Vector2.zero;
            IdleAnimation();
        }

    
        // jump
        if (Input.GetKeyDown(KeyCode.UpArrow) || jumping)                                          
        {
            if (wasRunningBeforeJump) {
                jumper.Jump();
            }
            else if (!wasRunningBeforeJump)
            {
                IdleAnimation();
                jumper.Jump();
            }
        }

        // shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
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

    private void RunAnimation()
    {
        animator.SetInteger("State", 2);
        wasRunningBeforeJump = true;
    }

    private void IdleAnimation()
    {
        wasRunningBeforeJump = false;
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

    public void moveCharLeft()
    {
        moveLeft = true;
    }

    public void stopMovingCharLeft()
    {
        moveLeft = false;
    }

    public void moveCharRight()
    {
        moveRight = true;
    }

    public void stopMovingCharRight()
    {
        moveRight = false;
    }

    public void startJumping()
    {
        jumping = true;
    }

    public void stopJumping()
    {
        jumping = false;
    }

    public void jump()
    {
        jumper.Jump();
    }
}
