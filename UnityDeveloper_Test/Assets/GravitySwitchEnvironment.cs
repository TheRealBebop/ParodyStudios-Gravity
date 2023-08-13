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
    }
}
