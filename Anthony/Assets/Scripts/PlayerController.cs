using UnityEngine;

using System.Collections;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public float jumpHeight;
    public float moveSpeed;

    bool isGrounded;
    bool hasLeftGround;
    Rigidbody2D rb2d;
    BoxCollider2D boxCollider;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

    }

    void Update()
    {

        GroundCheck();

        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb2d.velocity.y);
        rb2d.velocity = moveDir;

        if(Input.GetAxisRaw("Horizontal") == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(Input.GetAxisRaw("Horizontal") == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2d.AddForce(new Vector2(0, jumpHeight));
        }
    }

    /// Checks the ground and assigns IsGrounded
   
    private void GroundCheck()
    {
        // Cast a box below us just a hair to see if there's any objects below us
        var hits = Physics2D.BoxCastAll(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x * 0.9f, boxCollider.bounds.size.y), 0.0f, Vector2.down, 0.1f);

        // Check to see if any of the things we hit is in the Terrain layer and that we've already left the ground
        if (hits.Any(hit => hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) && hasLeftGround)
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

}

