using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int id;
    public Dictionary<GameObject, float> m_AdjacentNodeList = new Dictionary<GameObject, float>();

    public int adjSize = 0;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        adjSize = m_AdjacentNodeList.Count;

    }
}


public struct WeightToObject
{
    public GameObject gameobject;
    public float weight;
}