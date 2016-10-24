using System.Linq;

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ActorController : MonoBehaviour
{
    /// <summary>
    /// Speed of our actor's horizontal movement
    /// </summary>
    public float Speed = 1.0f;

    /// <summary>
    /// Is this actor currently on the ground?
    /// </summary>
    public bool IsGrounded { get; private set; }

    /// <summary>
    /// Velocity of this actor
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    /// Is this actor facing to the right? (Default pose)
    /// </summary>
    public bool FacingRight
    {
        get { return spriteRenderer.flipX == false; }
        set { spriteRenderer.flipX = !value; }
    }

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Tracker to make sure that we leave the ground
    /// </summary>
    private bool hasLeftGround;

    public Transform top_left;
    public Transform bot_right;
    public LayerMask layerMask;


    public void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void FixedUpdate()
    {
        // Check our ground below us
        IsGrounded = Physics2D.OverlapArea(top_left.position, bot_right.position, layerMask);

        if (Velocity != Vector2.zero)
        {
            // Change the Velocity of our actor
            rigidBody.velocity = Velocity;

            // If we're supposed to be going up, and we're already grounded, make sure we leave the ground
            if (Velocity.y > 0.0f && IsGrounded)
            {
                IsGrounded = false;
                hasLeftGround = false;
            }
        }

        // Clear our our Velocity
        Velocity = Vector2.zero;
    }


    /// <summary>
    /// Move our actor in a desired direction
    /// </summary>
    /// <param name="direction">Positive is right, negative is left, 1.0f is the normal speed of the actor</param>
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
            Velocity = new Vector2(direction * Speed, rigidBody.velocity.y);
    }


}