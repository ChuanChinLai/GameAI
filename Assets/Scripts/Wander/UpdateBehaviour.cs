using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBehaviour : MonoBehaviour
{
    enum Behaviour
    {
        Wander, 
        LookingFor, 
        MoveTo, 
    }

    public float maxAcceleration = 10f;
    public float maxVelocity = 3.5f;
    public float turnSpeed = 20f;

    Rigidbody RB;
    Wander WD;
    MoveTo MT;

    
    Vector3 target;

    Behaviour behaviour = Behaviour.Wander;

    public int numSamplesForSmoothing = 5;
    private Queue<Vector2> velocitySamples = new Queue<Vector2>();

    // Use this for initialization
    void Start ()
    {
        RB = GetComponent<Rigidbody>();
        WD = GetComponent<Wander>();
        MT = GetComponent<MoveTo>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(behaviour);

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Debug.DrawRay(transform.position, forward, Color.green);

        RaycastHit hit;


        if (behaviour == Behaviour.LookingFor)
        {
            if (Physics.Raycast(transform.position, forward, out hit))
            {
                Vector3 HitForward = hit.transform.TransformDirection(Vector3.forward);

                RB.velocity = Vector3.zero;

                if (Vector3.Dot(forward, HitForward) > 0)
                {
                    transform.Rotate(Vector3.up, -5 * turnSpeed * Time.deltaTime);
                }
                else
                {
                    transform.Rotate(Vector3.up,  5 * turnSpeed * Time.deltaTime);
                }


                if (hit.distance > 5.0f)
                {
                    behaviour = Behaviour.MoveTo;
                    target = transform.position + forward * 5.0f;
                    MT.SetTarget(target);
                }

                lookWhereYoureGoing();

                return;
            }
        }


        if (behaviour == Behaviour.MoveTo)
        {
            Vector3 acceleration = MT.getSteering();

            Debug.Log("MoveTo: " + acceleration);

            RB.velocity += acceleration * Time.deltaTime;

            if (MT.GetTarget())
            {
                behaviour = Behaviour.Wander;  
            }

            if (RB.velocity.magnitude > maxVelocity)
            {
                RB.velocity = RB.velocity.normalized * maxVelocity;
            }


            lookWhereYoureGoing();

            return;
        }
        else
        {
            Vector3 acceleration = WD.getSteering();

            RB.velocity += acceleration * Time.deltaTime;

            if (Physics.Raycast(transform.position, forward, out hit))
            {
                if (hit.distance <= 2.0f)
                {
                    RB.velocity = Vector3.zero;
                    behaviour = Behaviour.LookingFor;
                    return;
                }
            }

            if (RB.velocity.magnitude > maxVelocity)
            {
                RB.velocity = RB.velocity.normalized * maxVelocity;
            }

            lookWhereYoureGoing();
        }


    }

    public void lookWhereYoureGoing()
    {
        Vector2 direction = new Vector2(RB.velocity.x, RB.velocity.z);

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
