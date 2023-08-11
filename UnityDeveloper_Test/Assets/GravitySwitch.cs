using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class GravitySwitch : MonoBehaviour
{
    [SerializeField] Vector3 goalposition;
    [SerializeField] Vector3 goalrotation;
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

    [SerializeField] float speed = 10f;
    float current = 0f, target = 1f;

    PlayerController controller;
    public CharacterController characterController;
    HoloGravity holo;
    public PlayerController player;
    Rigidbody rb;
    [SerializeField] GravitySwitchEnvironment env;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        holo = GetComponent<HoloGravity>();
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        //env = GetComponent<GravitySwitchEnvironment>();
    }

    // Update is called once per frame
    void Update()
    {       
            /*
            if(Input.GetButtonDown("Submit") && switchLeft == true)
            {
                Vector3 flyUp = new Vector3(transform.position.x, -1.65f, transform.position.z);

                current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, flyUp, curve.Evaluate(current));
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(goalrotation), curve.Evaluate(current));
                Debug.Log("Lerp done");
                if (transform.position == flyUp)
                {
                    gravityVelocity.x = Mathf.Sqrt((4 * 10) * -2 * 9.81f);
                    characterController.Move(gravityVelocity * Time.deltaTime);
                    Debug.Log("Pushing Left");
                }
            }
            */
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
        if (Input.GetButtonDown("Submit"))
        {
            if(switchLeft == true)
            {
                enterLeft = true;
                Debug.Log("pass to env left rotate");
                env.RotateLeft();
            }
        }
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
                    Debug.Log("Left Wall hit");
                    switchForward = false;
                    switchBack = false;
                    switchRight = false;
                    switchLeft = true;
                    //play hologram left animation
                }
                else if(Hit.collider.tag == "Right Wall")
                {
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
    private void PushLeft()
    {
        Vector3 fly = new Vector3(transform.position.x, -1.65f, transform.position.z);

        current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, fly, curve.Evaluate(current));
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(goalrotation), curve.Evaluate(current));
        Debug.Log("Lerp done");
        if (transform.position == fly)
        {
            gravityVelocity.x = Mathf.Sqrt((4 * 10) * -2 * 9.81f);
            characterController.Move(gravityVelocity * Time.deltaTime);
            Debug.Log("Pushing Left");
        }
    }
}
