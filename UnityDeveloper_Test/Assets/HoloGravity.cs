using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloGravity : MonoBehaviour
{
    [SerializeField] Animator holo;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Left"))
        {
            holo.SetTrigger("left");
        }

        if (Input.GetButtonDown("Right"))
        {
            holo.SetTrigger("right");
        }

        if (Input.GetButtonDown("Forward"))
        {
            holo.SetTrigger("forward");
        }

        if (Input.GetButtonDown("Back"))
        {
            holo.SetTrigger("back");
        }
    }
}
