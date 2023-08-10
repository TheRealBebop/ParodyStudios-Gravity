using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitch : MonoBehaviour
{
    public float rayRange = 25f;
    Vector3 actualLeft;
    Vector3 actualRight;
    Vector3 actualForward;
    Vector3 actualBack;

    PlayerController controller;
    HoloGravity holo;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        holo = GetComponent<HoloGravity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Left"))
        {
            
            //actualLeft = gameObject.transform.InverseTransformDirection(Vector3.left);
            CastLeft();
        }
        if (Input.GetButtonDown("Right"))
        {
            actualRight = gameObject.transform.InverseTransformDirection(Vector3.right);
            DrawRight();
        }
        if (Input.GetButtonDown("Forward"))
        {
            actualForward = gameObject.transform.InverseTransformDirection(Vector3.forward);
            DrawForward();
        }
        if (Input.GetButtonDown("Back"))
        {
            actualBack = gameObject.transform.InverseTransformDirection(Vector3.back);
            DrawBack();
        }
    }

    public void CastLeft()
    {
        actualLeft = -transform.right;
        Debug.DrawRay(transform.position, actualLeft * rayRange);

        RaycastHit leftHit;
        Ray leftRay = new Ray(transform.position, actualLeft);

        if(controller.IsGrounded())
        {
            Physics.Raycast(leftRay, out leftHit, rayRange);
            {
                if (leftHit.collider.tag == "Left Wall");
                {
                    Debug.Log("Left Ray cast");
                }
            }
        }
    }
    public void DrawRight()
    {
        actualRight = transform.right;
        Debug.DrawRay(transform.position, actualRight * rayRange);

        RaycastHit rightHit;
        Ray rightRay = new Ray(transform.position, actualRight);

        if (controller.IsGrounded())
        {
            Physics.Raycast(rightRay, out rightHit, rayRange);
            {
                if (rightHit.collider.tag == "Right Wall") ;
                {
                    Debug.Log("Right Ray cast");
                }
            }
        }
    }
    public void DrawForward()
    {
        actualForward = transform.forward;
        Debug.DrawRay(transform.position, actualForward * rayRange);

        RaycastHit forwardHit;
        Ray forwardRay = new Ray(transform.position, actualForward);

        if (controller.IsGrounded())
        {
            Physics.Raycast(forwardRay, out forwardHit, rayRange);
            {
                if (forwardHit.collider.tag == "Forward Wall") ;
                {
                    Debug.Log("Forward Ray cast");
                }
            }
        }
    }
    public void DrawBack()
    {
        actualBack = -transform.forward;
        Debug.DrawRay(transform.position, actualBack * rayRange);

        RaycastHit backHit;
        Ray backRay = new Ray(transform.position, actualBack);

        if (controller.IsGrounded())
        {
            Physics.Raycast(backRay, out backHit, rayRange);
            {
                if (backHit.collider.tag == "Back Wall") ;
                {
                    Debug.Log("Back Ray cast");
                }
            }
        }
    }
}
