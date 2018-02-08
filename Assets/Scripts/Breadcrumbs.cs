using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breadcrumbs : MonoBehaviour
{
    public GameObject prefab;
    float time = 30.0f;

    int count = 0;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    private void FixedUpdate()
    {
        Debug.Log(Time.fixedDeltaTime);

        if (time >= 0.0f && count == 0)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }

        time -= Time.fixedDeltaTime;

        count++;

        if (count >= 10)
            count = 0;
    }
}
