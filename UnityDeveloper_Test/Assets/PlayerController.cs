using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float movementSpeed = 4f;
    public float smoothTime = 0.1f;
    public Transform camera;
    [SerializeField] Rigidbody rigidBody;
    public float jumpForce = 50f;
    public bool grounded;
    float turnSmoothVelocity;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

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
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(grounded == true)
            {
                Debug.Log("Jumping");
                rigidBody.AddForce(transform.up * jumpForce);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
        Debug.Log("Landed");
    }

    /*private void OnCollisionStay(Collision collision)
    {
        grounded = true;
        Debug.Log("Grounded son");
    }*/

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
        Debug.Log("Not grounded");
    }
}
