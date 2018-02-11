using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public float wanderOffset = 1.5f;
    public float wanderRadius = 4;
    public float wanderRate = 0.4f;
    public float wanderOrientation = 0;


    float maxAcceleration;

    // Use this for initialization
    void Start ()
    {
        maxAcceleration = GetComponent<UpdateBehaviour>().maxAcceleration;
	}

    public Vector3 getSteering()
    {
        float characterOrientation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

        wanderOrientation += randomBinomial() * wanderRate;

        float targetOrientation = wanderOrientation + characterOrientation;

        Vector3 targetPosition = transform.position + (orientationToVector(characterOrientation) * wanderOffset);

        targetPosition = targetPosition + (orientationToVector(targetOrientation) * wanderRadius);

        Vector3 acceleration = targetPosition - transform.position;

        acceleration.y = 0;

        acceleration.Normalize();

        acceleration *= maxAcceleration;

        return acceleration;
    }

    float randomBinomial()
    {
        return Random.value - Random.value;
    }

    Vector3 orientationToVector(float orientation)
    {
        return new Vector3(Mathf.Cos(orientation), 0, Mathf.Sin(orientation));
    }
}
