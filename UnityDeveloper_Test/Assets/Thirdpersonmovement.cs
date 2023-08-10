using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Thirdpersonmovement : MonoBehaviour
{
    public Transform camera;

    CharacterController controller;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector2 movement;
    public float moveSpeed = 10f;

    public float jumpHeight;
    public float gravity;
    public bool isGrounded;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, .1f, 1);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -1;
        }

        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;
        if (direction.magnitude >= 0.1f)
        {
            //To move player in the direction of the camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y; // find the angle between player direction and camera direction

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); //changes angle from current angle to target angle;

            transform.rotation = Quaternion.Euler(0f, angle, 0f); //rotate player

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //find direction to move player forward

            controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2 * gravity);
        }
        if (velocity.y > -20)
        {
            velocity.y += (gravity * 10) * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

    }
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
}
