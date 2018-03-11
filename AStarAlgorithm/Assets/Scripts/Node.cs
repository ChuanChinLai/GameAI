using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int Id;
    public bool Visited { get; set; }
    public double StraightLineDistanceToEnd { get; set; }
    public double? MinCostToStart { get; set; }
    public Node NearestToStart { get; set; }

    public List<Edge> Connections = new List<Edge>();


    public int edgeSize = 0;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        edgeSize = Connections.Count;
    }


    public double StraightLineDistanceTo(GameObject end)
    {
        return Vector3.Distance(gameObject.transform.position, end.transform.position);
    }

}


public class Edge
{
 //   public double Length { get; set; }
    public double Cost { get; set; }
    public Node ConnectedNode { get; set; }
}