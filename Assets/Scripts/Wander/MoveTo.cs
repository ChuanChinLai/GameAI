using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    Vector3 Target;

    float maxAcceleration;

    // Use this for initialization
    void Start()
    {
        maxAcceleration = GetComponent<UpdateBehaviour>().maxAcceleration;
    }

    public Vector3 getSteering()
    {
        Vector3 acceleration = Target - transform.position;

        acceleration.y = 0;

        acceleration.Normalize();

        acceleration *= maxAcceleration;

        return acceleration;
    }

    public void SetTarget(Vector3 T)
    {
        Target = T;
    }

    public bool GetTarget()
    {
        return Vector3.Distance(transform.position, Target) <= 1.0f;
    }
}
