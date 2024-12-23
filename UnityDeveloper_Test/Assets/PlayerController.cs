using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator animator;
    public CharacterController controller;
    FallDamage kill;
    public float movementSpeed = 4f;
    public float smoothTime = 0.1f;
    public new Transform camera;
    [SerializeField] Rigidbody rigidBody;

    public bool jumping;
    float turnSmoothVelocity;
    Vector3 lastPosition;

    public float jumpHeight;
    public float gravity;
    public bool grounded;
    Vector3 velocity;

    bool isWalkingOnLeftWall;


    void Start()
    {
        kill = GetComponent<FallDamage>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        IsGrounded();
        IsMoving();

        lastPosition = transform.position;
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            //To move player in the direction of the camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y; // find the angle between player direction and camera direction

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime); //changes angle from current angle to target angle;

            transform.rotation = Quaternion.Euler(0f, angle, 0f); //rotate player

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //find direction to move player forward
            //Debug.Log("Move Direction:" + moveDirection);

            if (isWalkingOnLeftWall == true)
            {
                moveDirection = transform.up * -1;
            }
            controller.Move(moveDirection.normalized * movementSpeed * Time.deltaTime);
        }
        Jump();
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            animator.SetBool("Running", false);

            velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2 * gravity);
            Debug.Log("YIPEEEEEE");
        }
        if (velocity.y > -20)
        {
            velocity.y += (gravity * 10) * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    public void IsMoving()
    {
        if (IsGrounded() == true)
        {
            if (lastPosition.x != gameObject.transform.position.x)
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
        if (collision.collider.tag == "Outside")
        {
            Debug.Log("This is Game Over");
            kill.GameOver();
        }
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