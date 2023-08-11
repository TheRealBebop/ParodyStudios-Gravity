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
    public bool switchRight;
    public bool switchForward;
    public bool switchBack;

    Vector3 gravityVelocity;

    [SerializeField] float speed = 10f;
    float current = 0f, target = 1f;

    PlayerController controller;
    public CharacterController characterController;
    HoloGravity holo;
    public PlayerController player;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        holo = GetComponent<HoloGravity>();
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(new Vector3(-9.81f, 0f, 0f));
        /*
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
        */
        if (Input.GetButtonDown("Left"))
        {
            CastRay(-transform.right);
            if(Input.GetButtonDown("Submit") && switchLeft == true)
            {

            }
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
                }
                else if (Hit.collider.tag == "Forward Wall")
                {
                    Debug.Log("Forward wall hit");
                }
                else if (Hit.collider.tag == "Back Wall")
                {
                    Debug.Log("Back wall hit");
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
