using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateWander : MonoBehaviour
{
    public float maxVelocity = 3.5f;
    public float turnSpeed = 20f;

    Rigidbody RB;
    Wander WD;

    public bool smoothing = true;
    public int numSamplesForSmoothing = 5;
    private Queue<Vector2> velocitySamples = new Queue<Vector2>();


    // Use this for initialization
    void Start ()
    {
        RB = GetComponent<Rigidbody>();
        WD = GetComponent<Wander>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 acceleration = WD.getSteering();

        Debug.Log(RB.angularDrag);

        RB.velocity += acceleration * Time.deltaTime;

        if (RB.velocity.magnitude > maxVelocity)
        {
            RB.velocity = RB.velocity.normalized * maxVelocity;
        }

        lookWhereYoureGoing();
    }

    public void lookWhereYoureGoing()
    {
        Vector2 direction = new Vector2(RB.velocity.x, RB.velocity.z);

        if (smoothing)
        {
            if (velocitySamples.Count == numSamplesForSmoothing)
            {
                velocitySamples.Dequeue();
            }

            velocitySamples.Enqueue(RB.velocity);

            direction = Vector2.zero;

            foreach (Vector2 v in velocitySamples)
            {
                direction += v;
            }

            direction /= velocitySamples.Count;
        }

        lookAtDirection(direction);
    }

    public void lookAtDirection(Vector2 direction)
    {
        direction.Normalize();

        // If we have a non-zero direction then look towards that direciton otherwise do nothing
        if (direction.sqrMagnitude > 0.001f)
        {
            float toRotation = (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg);
            float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, toRotation, Time.deltaTime * turnSpeed);

            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }


}
