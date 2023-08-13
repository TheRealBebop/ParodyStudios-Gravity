using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GravitySwitch : MonoBehaviour
{
    [SerializeField] Transform parentPlayer;
    [SerializeField] Transform childEnv;

    [SerializeField] Vector3 goalROTATE;
    [SerializeField] private AnimationCurve curve;
    public float rayRange = 10000000f;
    Vector3 actualdirection;

    public int switchLeft = 0;
    public int switchRight = 0;
    public int switchForward = 0;
    public int switchBack = 0;

    private bool rotating = false;

    int enter = 0;

    [SerializeField] float speed = 0.1f;
    float current = 0f, target = 10f;

    int leftWallHit = 0;
    int rightWallHit = 0;
    int forwardWallHit = 0;
    int backWallHit = 0;
    int groundHit = 0;
    int ceilingHit = 0;

    public PlayerController controller;
    //public CharacterController characterController;
    HoloGravity holo;
    Rigidbody rb;
    //[SerializeField] GravitySwitchEnvironment env;

    Vector3 flyUp;
    Quaternion leftGoalRotation;
    Vector3 exactRotation;
    Vector3 oldRotation;
    Vector3 newRotation;
    Vector3 worldRotation;
    Quaternion goalRotation;
    bool rotatePlayerAndWorld = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        //characterController = GetComponent<CharacterController>();
        holo = GetComponent<HoloGravity>();
        rb = GetComponent<Rigidbody>();
        childEnv.parent = null;
        childEnv.transform.rotation = Quaternion.Euler(Vector3.zero);
        flyUp = new Vector3(transform.position.x, -12.34f, transform.position.z);
        //leftGoalRotation = Quaternion.Euler(0,0,0) * Quaternion.Euler(0, 0, -90f);

        //env = GetComponent<GravitySwitchEnvironment>();
    }

    // Update is called once per frame
    void Update()
    {       
        if(enter == 1 && (switchLeft == 1 || switchRight == 1 || switchForward == 1 || switchBack == 1))
        {
            Debug.Log("ExactRotation:" + exactRotation);
            if(switchLeft == 1)
                RotateToNearest90(-transform.right);
            if(switchRight == 1)
                RotateToNearest90(transform.right);
            if (switchForward == 1)
                RotateToNearest90(transform.forward);
            if (switchBack == 1)
                RotateToNearest90(-transform.forward);
        }

        
        //if (rotateToWall == true)
            //PlayerLerp(exactRotation);
            //RotateToGlobalUp();

        if (Input.GetButtonDown("Left"))
        {
            switchLeft = 1;
            //CastRay(-transform.right);
        }

        if (Input.GetButtonDown("Right"))
        {
            switchRight = 1;
            //CastRay(transform.right);
        }

        if (Input.GetButtonDown("Forward"))
        {
            switchForward = 1;
            //CastRay(transform.forward);
        }

        if (Input.GetButtonDown("Back"))
        {
            switchBack = 1;
            //CastRay(-transform.forward);
        }

        if (switchLeft == 1 || switchRight == 1 || switchBack == 1 || switchForward == 1)
        {
            if(Input.GetButtonDown("Submit"))
            {
                enter = 1;
                Debug.Log("Enter hit");
            }
        }
    }

    public void RotateToNearest90(Vector3 snap)
    {
        Vector3 localDirection = snap;
        Quaternion targetRotation = Quaternion.LookRotation(localDirection, transform.up);
        transform.rotation = targetRotation;

        Vector3 currentRotation = transform.eulerAngles;
        float newYRotation = Mathf.Round(currentRotation.y / 90.0f) * 90.0f;
        transform.eulerAngles = new Vector3(currentRotation.x, newYRotation, currentRotation.z);
        Debug.Log("Rotated to right angle");
        controller.enabled = false;
        FlyUp();
        if(switchForward == 1 || switchLeft == 1 || switchRight == 1 || switchBack == 1)
        {
            RotateUp();
            childEnv.parent = parentPlayer;
            controller.enabled = false;
            RotateDown();
            childEnv.parent = null;
            controller.enabled = true;
        }
        /*
        if(switchLeft == 1)
        {
            RotateLeft();
        }
        if(switchRight == 1)
        {
            RotateRight();
        }
        */
        enter = 0;
        switchLeft = 0;
        switchRight = 0;
        switchForward = 0;
        switchBack = 0;
        //GetOldRotation();
    }

    private void FlyUp()
    {
        transform.Translate(new Vector3(0f, 2f, 0f));
    }

    public void RotateUp()
    {
        Vector3 rotation = new Vector3(-90f, 0f, 0f);
        transform.Rotate(rotation);
    }
    public void RotateDown()
    {
            Vector3 rotation = new Vector3(90f, 0f, 0f);
            transform.Rotate(rotation);
    }
    /*
    public void RotateLeft()
    {
        if (rotatePlayerAndWorld == false)
        {
            Vector3 rotation = new Vector3(0f, 0f, -90f);
            transform.Rotate(rotation);
            //rotatePlayerAndWorld = true;
            //RotateRight();
        }
        else if (rotatePlayerAndWorld == true)
        {
            childEnv.parent = parentPlayer;
            Vector3 rotation = new Vector3(0f, 0f, -90f);
            transform.Rotate(rotation);
            childEnv.parent = null;
            controller.enabled = true;
            rotatePlayerAndWorld = false;
        }
    }
    public void RotateRight()
    {
        if (rotatePlayerAndWorld == false)
        {
            Vector3 rotation = new Vector3(0f, 0f, 90f);
            transform.Rotate(rotation);
            rotatePlayerAndWorld = true;
            RotateLeft();
        }
        else if (rotatePlayerAndWorld == true)
        {
            childEnv.parent = parentPlayer;
            Vector3 rotation = new Vector3(-90f, 0f, 90f);
            transform.Rotate(rotation);
            childEnv.parent = null;
            controller.enabled = true;
            rotatePlayerAndWorld = false;
        }
    }
    */
    /*
    private void GetOldRotation()
    {
        oldRotation = transform.rotation.eulerAngles;
        Debug.Log("Old rotation:" + oldRotation);
    }
    */

    /*
    public void PlayerLerp(Vector3 deg)
    {
        controller.enabled = false;
        //Debug.Log("DEG:" + deg);
       // Vector3 finalRotation = oldRotation - deg;
        //goalRotation = Quaternion.Euler(finalRotation);
        //Debug.Log("Goal Rotation:" + goalRotation.eulerAngles);

        if (transform.rotation != Quaternion.Euler(deg))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(deg), curve.Evaluate(current));
            Debug.Log("ZEROING");
        }
        PlayerFlyUp(deg);
    }
    private void PlayerFlyUp(Vector3 deg)
    {
        if (transform.position != flyUp)
        {
            current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, flyUp, curve.Evaluate(current));
            //Quaternion rotationToZero = Quaternion.Euler(leftGoalRotation);
            Debug.Log("Vector3 Lerp");
        }

        if (transform.rotation == Quaternion.Euler(deg))
        {
            /*
            newRotation = transform.rotation.eulerAngles;
            Debug.Log("New rotation:" + newRotation);
            worldRotation = newRotation - oldRotation;
            worldRotate = true;
            Debug.Log("Player Rotation done");
            Debug.Log("World Rotation: " + worldRotation);

            enter = 0;
            switchLeft = 0;
            switchRight = 0;
            switchForward = 0;
            switchBack = 0;

            leftWallHit = 0;
            rightWallHit = 0;
            forwardWallHit = 0;
            backWallHit = 0;
            groundHit = 0;
            ceilingHit = 0;
            RotateWorld();

        }
    }

    public void RotateWorld()
    {
        newRotation = transform.rotation.eulerAngles;
        Debug.Log("New rotation:" + newRotation);
        worldRotation = newRotation - oldRotation;
        worldRotate = true;
        Debug.Log("Player Rotation done");
        Debug.Log("World Rotation: " + worldRotation);

        if (worldRotate == true && !rotating)
        {
            rotating = true;

            childEnv.parent = parentPlayer;

            
            Quaternion worldGoalRotation = transform.rotation * Quaternion.Euler(worldRotation);
            Debug.Log("Rotated: " + worldRotation);
            float maxAngle = 90.0f * curve.Evaluate(current);
            
            Debug.Log("World rotation IS: " + oldRotation);
            StartCoroutine(RotateTowards());
        }
    }
    IEnumerator RotateTowards()
    {
        if(worldRotate == true)
        {
            while (transform.rotation != Quaternion.Euler(oldRotation))
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(oldRotation), curve.Evaluate(current));
                Debug.Log("Gotta rotate");
            }
            yield return null;
        }

        worldRotate = false;
        Debug.Log("World rotation done");
        childEnv.parent = null;
        controller.enabled = true;
        rotateToWall = false;
        rotating = false;
    }

   
    public void CastRay(Vector3 direction)
    {
        actualdirection = direction;
        Debug.DrawRay(transform.position, actualdirection * rayRange);

        RaycastHit Hit;
        Ray ray = new Ray(transform.position, actualdirection);

        if(controller.IsGrounded())
        {
            Physics.Raycast(ray, out Hit, rayRange);
            if(Hit.collider.tag == "Left Wall")
            {
                leftWallHit = 1;

                if(switchLeft == 1) 
                {
                    exactRotation = new Vector3 (0f, 0f, -90f);
                    Debug.Log("EXACT ROTATION : " + exactRotation);
                }
                if(switchRight == 1)
                {
                    exactRotation = new Vector3(0f, 180f, 90f);
                    Debug.Log("EXACT ROTATION : " + exactRotation);
                }
                if (switchForward == 1)
                {
                    exactRotation = new Vector3(-90f, -90f, 0f);
                    Debug.Log("EXACT ROTATION : " + exactRotation);
                }
                if (switchBack == 1)
                {
                    exactRotation = new Vector3(90f, 90f, 0f);
                    Debug.Log("EXACT ROTATION : " + exactRotation);
                }
            }
            else if(Hit.collider.tag == "Right Wall")
            {
                rightWallHit = 1;

                if (switchLeft == 1)
                {
                    exactRotation = new Vector3(0f, 180f, -90f);
                }
                if (switchRight == 1)
                {
                    exactRotation = new Vector3(90f, 0f, 90f);
                }
                if (switchForward == 1)
                {
                    exactRotation = new Vector3(-90f, 90f, 0f);
                }
                if (switchBack == 1)
                {
                    exactRotation = new Vector3(90f, -90f, 0f);
                }
            }
            else if(Hit.collider.tag == "Forward Wall")
            {

                forwardWallHit = 1;

                if (switchLeft == 1)
                {
                    exactRotation = new Vector3(0f, 90f, -90f);
                }
                if (switchRight == 1)
                {
                    exactRotation = new Vector3(0f, -90f, 90f);
                }
                if (switchForward == 1)
                {
                    exactRotation = new Vector3(90f, 0f, 180f);
                }
                if (switchBack == 1)
                {
                    exactRotation = new Vector3(90f, 180f, 0f);
                }
            }
            else if(Hit.collider.tag == "Back Wall")
            {

                backWallHit = 1;

                if (switchLeft == 1)
                {
                    exactRotation = new Vector3(0f, -90f, -90f);
                }
                if (switchRight == 1)
                {
                    exactRotation = new Vector3(0f, 90f, 90f);
                }
                if (switchForward == 1)
                {
                    exactRotation = new Vector3(-90f, 180f, 0f);
                }
                if (switchBack == 1)
                {
                    exactRotation = new Vector3(90f, 0f, 0f);
                }
            }
            else if (Hit.collider.tag == "Ground")
            {

                groundHit = 1;
                if (switchLeft == 1)
                {
                    exactRotation = new Vector3(0f, 90f, 0f);
                }
                if (switchRight == 1)
                {
                    exactRotation = new Vector3(0f, -90f, 0f);
                }
                if (switchForward == 1)
                {
                    exactRotation = new Vector3(0f, 0f, 0f);
                }
                if (switchBack == 1)
                {
                    exactRotation = new Vector3(0f, 180f, 0f);
                }
            }
            //CEILING DONE
            else if (Hit.collider.tag == "Ceiling")
            {

                ceilingHit = 1;
                if (switchLeft == 1)
                {
                    exactRotation = new Vector3(0f, 180f, -180f);
                }
                if (switchRight == 1)
                {
                    exactRotation = new Vector3(-180f, 180f, 0f);
                }
                if (switchForward == 1)
                {
                    exactRotation = new Vector3(0f, -90f, 180f);
                }
                if (switchBack == 1)
                {
                    exactRotation = new Vector3(0f, 90f, -180f);
                }
            }
        }
    }
    */
}
