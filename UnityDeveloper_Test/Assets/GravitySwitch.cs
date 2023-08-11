using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class GravitySwitch : MonoBehaviour
{
    [SerializeField] Transform parentObject;
    [SerializeField] Transform childObject;

    [SerializeField] Vector3 goalposition;
    [SerializeField] Vector3 leftGoalRotation;
    [SerializeField] private AnimationCurve curve;
    public float rayRange = 10000000f;
    Vector3 actualdirection;
    public bool switchLeft;
    public bool enterLeft;

    public bool switchRight;
    public bool enterRight;

    public bool switchForward;
    public bool enterForward;

    public bool switchBack;
    public bool enterBack;


    Vector3 gravityVelocity;

    [SerializeField] float speed = 0.1f;
    float current = 0f, target = 10f;

    public PlayerController controller;
    //public CharacterController characterController;
    HoloGravity holo;
    Rigidbody rb;
    [SerializeField] GravitySwitchEnvironment env;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        //characterController = GetComponent<CharacterController>();
        holo = GetComponent<HoloGravity>();
        rb = GetComponent<Rigidbody>();
        //env = GetComponent<GravitySwitchEnvironment>();
    }

    // Update is called once per frame
    void Update()
    {       
        if(Input.GetButtonDown("Submit") && switchLeft == true)
        {
            PlayerLerpLeft();
        }

        if (Input.GetButtonDown("Left"))
        {
            CastRay(-transform.right);
        }
        if (Input.GetButtonDown("Right"))
        {
            CastRay(transform.right);
        }
        if (Input.GetButtonDown("Forward"))
        {
            CastRay(transform.forward);
        }
        if (Input.GetButtonDown("Back"))
        {
            CastRay(-transform.forward);
        }
        
       // if (Input.GetButtonDown("Submit"))
        //{
          //  if(switchLeft == true)
            //{
                //transform.Rotate(Vector3.back, Time.deltaTime * 8f);
                //enterLeft = true;
                //Debug.Log("pass to env left rotate");
                //env.RotateLeft();
            //}
       // }
        
    }

    private void PlayerLerpLeft()
    {
        controller.enabled = false;
        Vector3 flyUp = new Vector3(transform.position.x, transform.up.y + 4f, transform.position.z);
        while(transform.position != flyUp)
        {
            current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, flyUp, curve.Evaluate(current));
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(leftGoalRotation/*CHANGE THIS TO PLAYER'S LOCAL LEFT ANGLE*/), curve.Evaluate(current));
            Debug.Log("Lerp done");
        }
        Vector3 rotationLeftAmount = new Vector3(0, 0, 90);
        RotateWorld(rotationLeftAmount);
        /*
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
        childObject.parent = parentObject;
        Quaternion worldGoalRotation = Quaternion.Euler(transform.rotation.eulerAngles + degreesToRotate);
        while(transform.rotation != worldGoalRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, worldGoalRotation, curve.Evaluate(current));
        }
        childObject.parent = null;
        controller.enabled = true;
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
            {
                if (Hit.collider.tag == "Left Wall")
                {
                    //childObject.parent = parentObject;
                    Debug.Log("Left Wall hit");
                    switchForward = false;
                    switchBack = false;
                    switchRight = false;
                    switchLeft = true;
                    //play hologram left animation
                }
                else if(Hit.collider.tag == "Right Wall")
                {
                    //childObject.parent = null;
                    Debug.Log("Right Wall hit");
                    switchForward = false;
                    switchBack = false;
                    switchRight = true;
                    switchLeft = false;
                    //play hologram left animation
                }
                else if (Hit.collider.tag == "Forward Wall")
                {
                    Debug.Log("Forward wall hit");
                    switchForward = true;
                    switchBack = false;
                    switchRight = false;
                    switchLeft = false;
                    //play hologram left animation
                }
                else if (Hit.collider.tag == "Back Wall")
                {
                    Debug.Log("Back wall hit");
                    switchForward = false;
                    switchBack = true;
                    switchRight = false;
                    switchLeft = false;
                    //play hologram left animation
                }
            }
        }
    }
    /*
    private void PushLeft()
    {
        Vector3 fly = new Vector3(transform.position.x, -1.65f, transform.position.z);

        current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, fly, curve.Evaluate(current));
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(leftGoalRotation), curve.Evaluate(current));
        Debug.Log("Lerp done");
        if (transform.position == fly)
        {
            gravityVelocity.x = Mathf.Sqrt((4 * 10) * -2 * 9.81f);
            characterController.Move(gravityVelocity * Time.deltaTime);
            Debug.Log("Pushing Left");
        }
    }
    */
}
