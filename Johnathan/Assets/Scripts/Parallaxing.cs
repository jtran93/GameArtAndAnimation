using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour
{

    public Transform[] backgrounds;                 //Array (list) of all the back- and foregrounds to be parallaxed
    private float[] parallaxScales;                 //The proportion of the camera's movement to move the backgrounds by.
    public float smoothing = 1f;                    //How smooth parallax is going to be. Set this above 0.

    private Transform cam;                          //reference to the main cameras transform
    private Vector3 previousCamPos;                 //store position of the camera in the previous frame

    void Awake()
    {
        //set up camera reference
        cam = Camera.main.transform;
    }

    void Start()
    {
        //Previous frame has current frame's camera pos
        previousCamPos = cam.position;
        parallaxScales = new float[backgrounds.Length];

        //assigning corresponding parallaxScales
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;

        }
    }

    void Update()
    {

        //for each background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            //the parallax is the opposite of the camera movement b/c the previous frame multipled by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            //set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //create a target position which is the background's current position with its target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

        }
        //set the previousCamPos to the camera's positino at the end of the frame
        previousCamPos = cam.position;
    }
}
