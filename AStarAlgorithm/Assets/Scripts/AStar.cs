using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public GameObject Map;
    string Path;

    public double ShortestPathCost { get; private set; }

    public GameObject StartObj;
    public GameObject EndObj;


    int NodeVisits = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Run AStar");

            var res = GetShortestPathAstart();

            foreach (Node node in res)
            {
                Debug.Log(node.Id);
            }

            Debug.Log("Node Visited: " + NodeVisits);
            Debug.Log("Total Distance: " + ShortestPathCost);
        }
    }


    private void BuildShortestPath(List<Node> list, Node node)
    {
        if (node.NearestToStart == null)
            return;
        list.Add(node.NearestToStart);
        ShortestPathCost += node.Connections.Single(x => x.ConnectedNode == node.NearestToStart).Cost;
        BuildShortestPath(list, node.NearestToStart);
    }


    public List<Node> GetShortestPathAstart()
    {
        Node End = EndObj.GetComponent<Node>();

        List<GameObject> NodeList = Map.GetComponent<GraphsGenerator>().NodeList;

        foreach (var GO in NodeList)
            GO.GetComponent<Node>().StraightLineDistanceToEnd = GO.GetComponent<Node>().StraightLineDistanceTo(EndObj);

        AstarSearch();
        var shortestPath = new List<Node>();
        shortestPath.Add(End);
        BuildShortestPath(shortestPath, End);
        shortestPath.Reverse();
        return shortestPath;
    }


    void AstarSearch()
    {
        NodeVisits = 0;
        ShortestPathCost = 0;

        Node Start = StartObj.GetComponent<Node>();
        Start.MinCostToStart = 0;

        Node End = EndObj.GetComponent<Node>();

        var prioQueue = new List<Node>();

        prioQueue.Add(Start);
        do
        {
            NodeVisits++;
            prioQueue = prioQueue.OrderBy(x => x.MinCostToStart.Value + x.StraightLineDistanceToEnd).ToList();
            var node = prioQueue.First();
            prioQueue.Remove(node);
            foreach (var cnn in node.Connections.OrderBy(x => x.Cost))
            {
                var childNode = cnn.ConnectedNode;
                if (childNode.Visited)
                    continue;
                if (childNode.MinCostToStart == null || node.MinCostToStart + cnn.Cost < childNode.MinCostToStart)
                {
                    childNode.MinCostToStart = node.MinCostToStart + cnn.Cost;
                    childNode.NearestToStart = node;
                    if (!prioQueue.Contains(childNode))
                        prioQueue.Add(childNode);
                }
            }
            node.Visited = true;
            if (node == End)
                return;
        } while (prioQueue.Any());
    }
}
