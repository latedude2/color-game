using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField]
    float turnSpeed = 80.0f;

    public float GetTurnSpeed()
    {
        return turnSpeed;
    }
}
