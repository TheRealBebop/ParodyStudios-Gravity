using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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

    bool leftWallHit = false;
    bool rightWallHit = false;
    bool forwardWallHit = false;
    bool backWallHit = false;
    bool groundHit = false;
    bool ceilingHit = false;

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
    bool worldRotate = false;


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
        if(enter == 1 && switchLeft == 1 /*|| switchRight == 1 || switchBack == 1 || switchForward == 1)*/ )
        {
            Debug.Log("ExactRotation:" + exactRotation);
            PlayerLerp(exactRotation);
        }

        //enter = 0;
        //switchLeft = 0;

        if (Input.GetButtonDown("Left"))
        {
            switchLeft = 1;
            CastRay(-transform.right);
        }

        if (Input.GetButtonDown("Right"))
        {
            switchRight = 1;
            CastRay(transform.right);
        }

        if (Input.GetButtonDown("Forward"))
        {
            switchForward = 1;
            CastRay(transform.forward);
        }

        if (Input.GetButtonDown("Back"))
        {
            switchBack = 1;
            CastRay(-transform.forward);
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

    private void PlayerLerp(Vector3 deg)
    {
        oldRotation = new Vector3 (transform.rotation.x, transform.rotation.y, transform.rotation.z);
        controller.enabled = false;
        if (transform.position != flyUp)
        {
            current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, flyUp, curve.Evaluate(current));
            //Quaternion rotationToZero = Quaternion.Euler(leftGoalRotation);
            Debug.Log("Vector3 Lerp");
        }
        Debug.Log("deg:" + deg);
        Quaternion goalRotation = Quaternion.Euler(0, 0, 0) * Quaternion.Euler(deg);

        if (transform.rotation != goalRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRotation, curve.Evaluate(current));
            Debug.Log("ZEROING");
        }

        if (transform.rotation == goalRotation)
        {
            enter = 0;
            switchLeft = 0;

            leftWallHit = false;
            rightWallHit = false;
            forwardWallHit = false;
            backWallHit = false;
            groundHit = false;
            ceilingHit = false;

            newRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
            worldRotation = newRotation - oldRotation;
            worldRotate = true;
            Debug.Log("Player Rotation done");
        }
        RotateWorld(worldRotation);
        /*
        controller.enabled = false;
        Vector3 flyUp = new Vector3(transform.position.x, -12.34f, transform.position.z);
        while(transform.position != flyUp)
        {
            current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, flyUp, curve.Evaluate(current));
            Quaternion leftGoalRotation = transform.rotation * Quaternion.Euler(0, 0, 90);
            transform.rotation = Quaternion.Lerp(transform.rotation, leftGoalRotation, curve.Evaluate(current));
            Debug.Log("Lerp done");
        }
        Vector3 rotationLeftAmount = new Vector3(0, 0, 90);
        //RotateWorld(rotationLeftAmount);
        if (transform.position == flyUp)
        {
            gravityVelocity.x = Mathf.Sqrt((4 * 10) * -2 * 9.81f);
            characterController.Move(gravityVelocity * Time.deltaTime);
            Debug.Log("Pushing Left");
        }
        */
    }
    public void RotateWorld(Vector3 degreesToRotate)
    {
        if (worldRotate == true && !rotating)
        {
            rotating = true;

            childEnv.parent = parentPlayer;

            Quaternion worldGoalRotation = transform.rotation * Quaternion.Euler(0, 0, 90f);
            float maxAngle = 90.0f * curve.Evaluate(current);

            StartCoroutine(RotateTowards(worldGoalRotation, maxAngle));
        }
    }

    IEnumerator RotateTowards(Quaternion targetRotation, float maxAngle)
    {
        if(worldRotate == true)
        {
            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxAngle * Time.deltaTime);
                yield return null;
            }
        }

        worldRotate = false;
        Debug.Log("World rotation done");
        childEnv.parent = null;
        controller.enabled = true;

        rotating = false;
    }
    /*
    childEnv.parent = parentPlayer;
    //Vector3 targetEulerAngles = transform.rotation.eulerAngles + new Vector3(0, 0, 90.1f);
    //targetEulerAngles.x = 0;
    //targetEulerAngles.y = 0;
    //Quaternion worldGoalRotation = Quaternion.Euler(targetEulerAngles);
    //degreesToRotate = new Vector3(0, 0, 90f);
    Quaternion worldGoalRotation = transform.rotation * Quaternion.Euler(0,0,90f);
    float maxAngle = 90.0f * curve.Evaluate(current);

    //worldGoalRotation.x = 0;
    //worldGoalRotation.y = 0;
    while (transform.rotation != worldGoalRotation)
    {
        //transform.Rotate(degreesToRotate, Space.World);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, worldGoalRotation, maxAngle);
        Debug.Log("rotating");
    }
    Debug.Log("World rotation done");
    childEnv.parent = null;
    controller.enabled = true;
    */
    /*
    childEnv.parent = parentPlayer;
    Vector3 targetEulerAngles = transform.rotation.eulerAngles + new Vector3(0, 0, 90.1f);
    //targetEulerAngles.x = 0;
    //targetEulerAngles.y = 0;
    Quaternion worldGoalRotation = Quaternion.Euler(targetEulerAngles);
    //worldGoalRotation = transform.rotation * Quaternion.Euler(0, 0, 90);
    float maxAngle = 90.1f * curve.Evaluate(current);
    //worldGoalRotation.x = 0;
    //worldGoalRotation.y = 0;
    while (transform.rotation != worldGoalRotation)
    {
        //transform.Rotate(degreesToRotate, Space.World);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, worldGoalRotation, curve.Evaluate(current));
    }
    Debug.Log("World rotation done");
    childEnv.parent = null;
    controller.enabled = true;
    */


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
                leftWallHit = true;
                rightWallHit = false;
                forwardWallHit = false;
                backWallHit = false;
                groundHit = false;
                ceilingHit = false;
                if(switchLeft == 1) 
                {
                    exactRotation =new Vector3 (0f, 0f, -90f);
                    Debug.Log("EXACT ROTATION : " + exactRotation);
                }
                if(switchRight == 1) 
                {
                    exactRotation = new Vector3(0f, 0f, 90f);
                    Debug.Log("EXACT ROTATION : " + exactRotation);
                }
            }
            else if(Hit.collider.tag == "Right Wall")
            {
                leftWallHit = false;
                rightWallHit = true;
                forwardWallHit = false;
                backWallHit = false;
                groundHit = false;
                ceilingHit = false;
                if (switchLeft == 1)
                {
                    exactRotation = new Vector3(0f, 0f, 90f);
                }
                if(switchRight == 1)
                {
                    exactRotation = new Vector3(0f, 0f, -90f);
                }
            }
            else if(Hit.collider.tag == "Forward Wall")
            {
                leftWallHit = false;
                rightWallHit = false;
                forwardWallHit = true;
                backWallHit = false;
                groundHit = false;
                ceilingHit = false;
                if (switchLeft == 1)
                {
                    exactRotation = new Vector3(-90f, 0f, 0f);
                }
            }
            else if(Hit.collider.tag == "Back Wall")
            {
                leftWallHit = false;
                rightWallHit = false;
                forwardWallHit = false;
                backWallHit = true;
                groundHit = false;
                ceilingHit = false;
                if (switchLeft == 1)
                {
                    exactRotation = new Vector3(90f, 0f, 0f);
                }
            }
            else if (Hit.collider.tag == "Ground")
            {
                leftWallHit = false;
                rightWallHit = false;
                forwardWallHit = false;
                backWallHit = false;
                groundHit = true;
                ceilingHit = false;
                if (switchLeft == 1)
                {
                    exactRotation = new Vector3(0f, 90f, 90f);
                }
            }
            else if (Hit.collider.tag == "Ceiling")
            {
                leftWallHit = false;
                rightWallHit = false;
                forwardWallHit = false;
                backWallHit = false;
                groundHit = false;
                ceilingHit = true;
                if (switchLeft == 1)
                {
                    exactRotation = new Vector3(180f, 90f, 90f);
                }
            }

            /*
            if (Hit.collider.tag == "Left Wall")
            {
                //childObject.parent = parentObject;
                Debug.Log("Left Wall hit");
                switchForward = false;
                switchBack = false;
                switchRight = false;
                switchLeft = 1;
                Debug.Log("switch left is 1");
                //enter = true;
                //play hologram left animation
            }
            else if(Hit.collider.tag == "Right Wall")
            {
                //childObject.parent = null;
                Debug.Log("Right Wall hit");
                switchForward = false;
                switchBack = false;
                switchRight = true;
                switchLeft = 0;
                //play hologram left animation
            }
            else if (Hit.collider.tag == "Forward Wall")
            {
                Debug.Log("Forward wall hit");
                switchForward = true;
                switchBack = false;
                switchRight = false;
                switchLeft = 0;
                //play hologram left animation
            }
            else if (Hit.collider.tag == "Back Wall")
            {
                Debug.Log("Back wall hit");
                switchForward = false;
                switchBack = true;
                switchRight = false;
                switchLeft = 0;
                //play hologram left animation
            }
            */
        }
    }
}
