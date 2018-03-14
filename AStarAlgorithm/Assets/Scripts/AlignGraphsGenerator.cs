using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignGraphsGenerator : MonoBehaviour
{
    public GameObject NodeGameObject;

    public int NumNodesInEdge = 2;
    public List<GameObject> NodeList = new List<GameObject>();

    public float[,] graph;
    private int nodeID = 0;

    GameObject snake;

    // Use this for initialization
    void Start()
    {
        int size = (2 * NumNodesInEdge + 1) * (2 * NumNodesInEdge + 1);
        graph = new float[size, size];

        CreateGraphs();
    }

    // Update is called once per frame
    void Update()
    {
        if(snake == null)
        {
            snake = GameObject.Find("Snake_head");
        }
    }

    void CreateGraphs()
    {
        for (int i = -NumNodesInEdge; i <= NumNodesInEdge; i++)
        {
            for (int j = -NumNodesInEdge; j <= NumNodesInEdge; j++)
            {
                Vector3 pos = new Vector3(i, 0, j);
                GameObject GO = Instantiate(NodeGameObject, pos, Quaternion.identity);
                GO.name = "Node" + nodeID;
                GO.GetComponent<Node>().Id = nodeID++;
                NodeList.Add(GO);
            }
        }

        CalculateEdges();
    }



    public void CalculateEdges()
    {
        for (int i = 0; i < NodeList.Count; i++)
        {
            NodeList[i].GetComponent<Node>().Connections.Clear();

            RaycastHit hit;

            {
                Vector3 up = NodeList[i].transform.TransformDirection(Vector3.up);

                if (Physics.Raycast(NodeList[i].transform.position, up, out hit, 5) && hit.transform.gameObject.tag == "Player" && Vector3.Distance(hit.transform.position, snake.transform.position) > 0.1f)
                {
                    continue;
                }
            }


            {
                Vector3 fwd = NodeList[i].transform.TransformDirection(Vector3.forward);

                if (Physics.Raycast(NodeList[i].transform.position, fwd, out hit, 1))
                {
                    Edge edge = new Edge();
                    edge.Cost = 1;
                    edge.ConnectedNode = hit.transform.gameObject.GetComponent<Node>();
                    NodeList[i].GetComponent<Node>().Connections.Add(edge);
//                    graph[i, hit.transform.gameObject.GetComponent<Node>().Id] = 1;
                }
            }

            {
                Vector3 back = NodeList[i].transform.TransformDirection(Vector3.back);

                if (Physics.Raycast(NodeList[i].transform.position, back, out hit, 1))
                {
                    Edge edge = new Edge();
                    edge.Cost = 1;
                    edge.ConnectedNode = hit.transform.gameObject.GetComponent<Node>();
                    NodeList[i].GetComponent<Node>().Connections.Add(edge);
 //                   graph[i, hit.transform.gameObject.GetComponent<Node>().Id] = 1;
                }
            }

            {
                Vector3 right = NodeList[i].transform.TransformDirection(Vector3.right);

                if (Physics.Raycast(NodeList[i].transform.position, right, out hit, 1))
                {
                    Edge edge = new Edge();
                    edge.Cost = 1;
                    edge.ConnectedNode = hit.transform.gameObject.GetComponent<Node>();
                    NodeList[i].GetComponent<Node>().Connections.Add(edge);
//                    graph[i, hit.transform.gameObject.GetComponent<Node>().Id] = 1;
                }
            }

            {
                Vector3 left = NodeList[i].transform.TransformDirection(Vector3.left);

                if (Physics.Raycast(NodeList[i].transform.position, left, out hit, 1))
                {
                    Edge edge = new Edge();
                    edge.Cost = 1;
                    edge.ConnectedNode = hit.transform.gameObject.GetComponent<Node>();
                    NodeList[i].GetComponent<Node>().Connections.Add(edge);
//                    graph[i, hit.transform.gameObject.GetComponent<Node>().Id] = 1;
                }
            }

        }
    }


    public void ResetNodes()
    {
        foreach (GameObject GO in NodeList)
        {
            GO.GetComponent<Node>().Visited = false;
            GO.GetComponent<Node>().NearestToStart = null;
            GO.GetComponent<Node>().MinCostToStart = null;
        }
    }
}