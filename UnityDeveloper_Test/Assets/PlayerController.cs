using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator animator;
    public CharacterController controller;
    public float movementSpeed = 4f;
    public float smoothTime = 0.1f;
    public Transform camera;
    [SerializeField] Rigidbody rigidBody;

    public float jumpForce = 50f;
    public bool grounded;
    public bool jumping;
    float turnSmoothVelocity;
    Vector3 lastPosition;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        lastPosition = transform.position;
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            //To move player in the direction of the camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y; // find the angle between player direction and camera direction
           
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime); //changes angle from curent angle to target angle;
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f); //rotate player
            
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //find direction to move player forward
            
            if(grounded == true)
            {
                controller.Move(moveDirection.normalized * movementSpeed * Time.deltaTime);
                //animator.SetTrigger("Running");
            }
        }        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);
    }

    public void Update()
    {
        IsMoving();
        IsGrounded();
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            animator.SetBool("Running", false);
            Jump();
        }
    }

    private void Jump()
    {
        Debug.Log("Jumping");
        rigidBody.AddForce(transform.up * jumpForce);
    }

    public void IsMoving()
    {
        if(IsGrounded() == true)
        {
            if(lastPosition.x != gameObject.transform.position.x)
            {
                animator.SetBool("Running", true);
            }
            else 
            {
                animator.SetBool("Running", false);
            }
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }

    public bool IsGrounded()
    {
        if (grounded == true)
        {
            animator.SetBool("Grounded", true);
            return true;
        }
        else
        {
            animator.SetBool("Grounded", false);
            return false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
        jumping = false;
        Debug.Log("Landed");
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
        jumping = true;
        Debug.Log("Not grounded");
        animator.SetBool("Running", false);
        animator.SetBool("Grounded", false);
    }
}