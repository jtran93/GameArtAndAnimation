using System.Linq;

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float walkSpeed = 2.0f;
    private float wallLeft = 0.0f;
    private float wallRight = 5.0f;
    private float patrolWidth = 1.05f;
    bool facingRight = true;
    Vector3 walkAmount;
    Transform myTrans;

    void Start()
    {
        wallLeft = transform.position.x - patrolWidth/2;
        wallRight = transform.position.x + patrolWidth/2;
        myTrans = this.transform;
    }

    void Update()
    {
       
        walkAmount.x = walkSpeed * Time.deltaTime;
        if (facingRight && transform.position.x >= wallRight)
        {
            Vector3 currRot = myTrans.eulerAngles;
            currRot.y += 180;
            myTrans.eulerAngles = currRot;
            facingRight = false;
        }

        else if (!facingRight && transform.position.x <= wallLeft)
        {
            Vector3 currRot = myTrans.eulerAngles;
            currRot.y += 180;
            myTrans.eulerAngles = currRot;
            facingRight = true;
        }
        transform.Translate(walkAmount);
    }
}