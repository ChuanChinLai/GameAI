using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SystemIO : MonoBehaviour
{
    string path = "C:\\Users\\u1070737\\Desktop\\Test2.txt";

    Kinematic Kinematic;
    StreamWriter sw;

    float time = 10.0f;

    // Use this for initialization
    void Start()
    {
        Kinematic = GetComponent<Kinematic>();

        if (!File.Exists(path))
        {
            sw = File.CreateText(path);
        }
    }


    private void FixedUpdate()
    {
        Debug.Log(Time.fixedDeltaTime);

        if (time >= 0.0f && sw != null)
        {
            Vector2 v = new Vector2(Kinematic.getVelocity().x, Kinematic.getVelocity().z);
            Debug.Log(v.magnitude);
            sw.WriteLine(v.magnitude);
        }
        else
        {
            sw.Close();
            Debug.Log("Done");
        }


        time -= Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update ()
    {
		



	}
}
