using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    int leftWallHit = 0;
    int rightWallHit = 0;
    int forwardWallHit = 0;
    int backWallHit = 0;
    int groundHit = 0;
    int ceilingHit = 0;

    int flag = 0;

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
        if(enter == 1 && (switchLeft == 1 || switchRight == 1 || switchForward == 1 || switchBack == 1))
        {
            Debug.Log("ExactRotation:" + exactRotation);
            PlayerLerp(exactRotation);
            PlayerFlyUp(exactRotation);
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

    public Quaternion PlayerLerp(Vector3 deg)
    {
        controller.enabled = false;
        goalRotation = Quaternion.Euler(0, 0, 0) * Quaternion.Euler(deg);

        if (transform.rotation != goalRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRotation, curve.Evaluate(current));
            Debug.Log("ZEROING");
        }
        return goalRotation;
    }

    private void PlayerFlyUp(Vector3 deg)
    {
        if (transform.rotation == goalRotation && transform.position != flyUp)
        {
            current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, flyUp, curve.Evaluate(current));
            //Quaternion rotationToZero = Quaternion.Euler(leftGoalRotation);
            Debug.Log("Vector3 Lerp");
        }
        Debug.Log("deg:" + deg);

        if (transform.rotation == goalRotation)
        {

            newRotation = transform.rotation.eulerAngles;
            Debug.Log("New rotation:" + newRotation);
            worldRotation = newRotation - oldRotation;
            worldRotate = true;
            Debug.Log("Player Rotation done");
            Debug.Log("World Rotation: " + worldRotation);
            RotateWorld(worldRotation * -1);

            enter = 0;
            switchLeft = 0;

            leftWallHit = 0;
            rightWallHit = 0;
            forwardWallHit = 0;
            backWallHit = 0;
            groundHit = 0;
            ceilingHit = 0;
            flag = 0;
        }
    }


    public void RotateWorld(Vector3 degreesToRotate)
    {
        if (worldRotate == true && !rotating)
        {
            rotating = true;

            childEnv.parent = parentPlayer;

            Quaternion worldGoalRotation = transform.rotation * Quaternion.Euler(degreesToRotate);
            Debug.Log("Rotated: " + degreesToRotate);
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
                    exactRotation = new Vector3(0f, 0f, 90f);
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
                    exactRotation = new Vector3(-90f, 0f, 0f);
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
                    exactRotation = new Vector3(-90f, 0f, -180f);
                }
                if (switchBack == 1)
                {
                    exactRotation = new Vector3(180f, -90f, 0f);
                }
            }
        }
    }
    private void GetOldRotation()
    {
        oldRotation = transform.rotation.eulerAngles;
        Debug.Log("Old rotation:" + oldRotation);
    }
}
