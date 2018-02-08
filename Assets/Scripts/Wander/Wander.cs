using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public float wanderOffset = 1.5f;
    public float wanderRadius = 4;
    public float wanderRate = 0.4f;
    public float wanderOrientation = 0;

    public float maxAcceleration = 10f;

    // Use this for initialization
    void Start ()
    {
		
	}

    public Vector3 getSteering()
    {
        float characterOrientation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

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
