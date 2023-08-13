using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloAnim : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject holo;

    private void Start()
    {
        holo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Left"))
        {
            holo.SetActive(true);
            anim.SetTrigger("left");
            anim.ResetTrigger("right");
            anim.ResetTrigger("forward");
            anim.ResetTrigger("back");
        }

        if (Input.GetButtonDown("Right"))
        {
            holo.SetActive(true);
            anim.SetTrigger("right");
            anim.ResetTrigger("left");
            anim.ResetTrigger("forward");
            anim.ResetTrigger("back");
        }

        if (Input.GetButtonDown("Forward"))
        {
            holo.SetActive(true);
            anim.SetTrigger("forward");
            anim.ResetTrigger("right");
            anim.ResetTrigger("left");
            anim.ResetTrigger("back");
        }

        if (Input.GetButtonDown("Back"))
        {
            holo.SetActive(true);
            anim.SetTrigger("back");
            anim.ResetTrigger("right");
            anim.ResetTrigger("forward");
            anim.ResetTrigger("left");
        }
        if(Input.GetButtonDown("Submit"))
        {
            anim.ResetTrigger("left");
            anim.ResetTrigger("right");
            anim.ResetTrigger("forward");
            anim.ResetTrigger("back");
            holo.SetActive(false);
        }
    }
}
