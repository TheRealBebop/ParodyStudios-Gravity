using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GravitySwitch : MonoBehaviour
{
    [SerializeField] Transform parentPlayer;
    [SerializeField] Transform childEnv;

    [SerializeField] Vector3 goalROTATE;
    [SerializeField] private AnimationCurve curve;
    public float rayRange = 10000000f;

    public int switchLeft = 0;
    public int switchRight = 0;
    public int switchForward = 0;
    public int switchBack = 0;

    int enter = 0;

    [SerializeField] float speed = 0.1f;

    public PlayerController controller;
    Vector3 exactRotation;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        childEnv.parent = null;
        childEnv.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

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


        if (Input.GetButtonDown("Left"))
        {
            switchLeft = 1;
        }

        if (Input.GetButtonDown("Right"))
        {
            switchRight = 1;
        }

        if (Input.GetButtonDown("Forward"))
        {
            switchForward = 1;
        }

        if (Input.GetButtonDown("Back"))
        {
            switchBack = 1;
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
        enter = 0;
        switchLeft = 0;
        switchRight = 0;
        switchForward = 0;
        switchBack = 0;
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
}
