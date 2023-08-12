using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GravitySwitchEnvironment : MonoBehaviour
{
    [SerializeField] AnimationCurve curveEnv;

    [SerializeField] GravitySwitch grav;
    [SerializeField] PlayerController player;

    float current = 0, target = 2000, speed = 0.1f;
    Vector3 rotateLeft;
    Vector3 flyUp;

    void Start()
    {
        rotateLeft = new Vector3(0, 0, 90);
    }

    void Update()
    {
        /*
        if(grav.enterLeft == true)
        {
            //player.enabled = false;
            Vector3 flyUp = new Vector3(transform.position.x, -3f, transform.position.z);

            current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, flyUp, curveEnv.Evaluate(current));
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotateLeft), curveEnv.Evaluate(current));
            Debug.Log("Lerp done");
            if(transform.position == flyUp)
            {
                player.enabled = true;
                player.gravity = -9.81f;
            //grav.enterLeft = false;
            }
        }
        grav.enterLeft = true;
    }

    public void RotateLeft()
    {
        if (grav.enterLeft == true)
        {
            Transform plr = player.transform;
            flyUp = new Vector3(transform.position.x, -9f, transform.position.z);
            while(transform.position != flyUp)
            {
                player.enabled = false;
                current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, flyUp, curveEnv.Evaluate(current));
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotateLeft), curveEnv.Evaluate(current));
                Debug.Log("Lerp done");
            }
        }
        if (transform.position == flyUp)
        {
            player.enabled = true;
            player.gravity = -9.81f;
            //grav.enterLeft = false;
        }
        grav.enterLeft = true;
        */
    }
}
