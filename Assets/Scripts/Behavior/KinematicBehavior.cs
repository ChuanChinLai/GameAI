using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicBehavior : MonoBehaviour {

    private Kinematic char_kinematic;
    private KinematicSteering ks;
    private DynoSteering ds;
    private KinematicSteeringOutput kso;
    private KinematicSeek seek;
    private KinematicArrive arrive;

    private Wander wander;

    private KinematicSteering seeking_output;
    private Vector3 new_velocity;

    // Use this for initialization
    void Start () {
        char_kinematic = GetComponent<Kinematic>();
        seek = GetComponent<KinematicSeek>();
        arrive = GetComponent<KinematicArrive>();

        wander = GetComponent<Wander>();
    }
	
	// Update is called once per frame
	void Update () {
        ks = new KinematicSteering();
        ds = new DynoSteering();

        // Decide on behavior
        //seeking_output = seek.updateSteering();
        //seeking_output = seek.getSteering();


        //seeking_output = arrive.getSteering();
        //char_kinematic.setVelocity(seeking_output.velc);


        //// Manually set orientation for now
        //float new_orient = char_kinematic.getNewOrientation(seeking_output.velc);
        //char_kinematic.setOrientation(new_orient);
        //char_kinematic.setRotation(0f);


        Vector3 acceleration = wander.getSteering();
        ds.force = acceleration;

        // Update Kinematic Steering
        kso = char_kinematic.updateSteering(ds, Time.deltaTime);
        
        transform.position = new Vector3(kso.position.x, transform.position.y, kso.position.z);


        Vector2 direction = new Vector2(char_kinematic.getVelocity().x, char_kinematic.getVelocity().z);
        float turnSpeed = 20.0f;

        direction.Normalize();

        // If we have a non-zero direction then look towards that direciton otherwise do nothing
        if (direction.sqrMagnitude > 0.001f)
        {
            float toRotation = (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg);
            float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, toRotation, Time.deltaTime * turnSpeed);

            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }

//        transform.rotation = Quaternion.Euler(0f, kso.orientation * Mathf.Rad2Deg, 0f);
    }

    private void kinematicSeekBehavior()
    {
        ds = new DynoSteering();

        // Decide on behavior
        seeking_output = seek.getSteering();
        char_kinematic.setVelocity(seeking_output.velc);
        // Manually set orientation for now
        float new_orient = char_kinematic.getNewOrientation(seeking_output.velc);
        char_kinematic.setOrientation(new_orient);
        char_kinematic.setRotation(0f);

        // Update Kinematic Steering
        kso = char_kinematic.updateSteering(ds, Time.deltaTime);

        transform.position = new Vector3(kso.position.x, transform.position.y, kso.position.z);
        transform.rotation = Quaternion.Euler(0f, kso.orientation * Mathf.Rad2Deg, 0f);
    }
}
