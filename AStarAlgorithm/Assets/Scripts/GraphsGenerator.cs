using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;



public class GraphsGenerator : MonoBehaviour
{
    public GameObject NodeGameObject;

    public int NumNodes = 5;
    public int MaxNumAdjacentEdges = 10;

    public float BoxSize = 10.0f;
    public float MaxDistanceToBeNeighbor = 10.0f;

    public float[,] graph;

    public List<GameObject> NodeList = new List<GameObject>();


    private int StartID = 0;

    // Use this for initialization
    void Start ()
    {
        graph = new float[NumNodes, NumNodes];

        CreateGraphs();
    }

    // Update is called once per frame
    void Update()
    {
//        DisplayGraphs();

        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    Debug.Log("Output Node");

        //    OutputToDisk();
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Debug.Log("Reset the Nodes");

        //    ResetNodes();
        //}
    }


    void CreateGraphs()
    {
        for(int i = 0; i < NumNodes; i++)
        {

            Vector3 pos = new Vector3(Random.Range(-BoxSize, BoxSize), Random.Range(-BoxSize, BoxSize), Random.Range(-BoxSize, BoxSize));
            GameObject GO = Instantiate(NodeGameObject, pos, Quaternion.identity);
            GO.name = "Node" + StartID;
            GO.GetComponent<Node>().Id = StartID++;
            NodeList.Add(GO);
        }


        for (int i = 0; i < NodeList.Count; i++)
        {
            Debug.Assert(NumNodes > MaxNumAdjacentEdges);

            List<GameObject> tmpList = new List<GameObject>(NodeList);
            tmpList.RemoveAt(i);

            int NumAdjacentEdges = Random.Range(1, MaxNumAdjacentEdges);

            while (NodeList[i].GetComponent<Node>().Connections.Count < NumAdjacentEdges && tmpList.Count != 0)
            {
                int index = Random.Range(0, tmpList.Count);

                if (tmpList[index].GetComponent<Node>().Connections.Count >= MaxNumAdjacentEdges)
                    continue;

                float dis = Vector3.Distance(NodeList[i].transform.position, tmpList[index].transform.position);

                if (dis < MaxDistanceToBeNeighbor)
                {
                    graph[i, tmpList[index].GetComponent<Node>().Id] = dis;
                    graph[tmpList[index].GetComponent<Node>().Id, i] = dis;

                    {
                        Edge edge = new Edge();
                        edge.Cost = dis;
                        edge.ConnectedNode = tmpList[index].GetComponent<Node>();

                        if(!NodeList[i].GetComponent<Node>().Connections.Contains(edge))
                            NodeList[i].GetComponent<Node>().Connections.Add(edge);
                    }

                    {
                        Edge edge = new Edge();
                        edge.Cost = dis;
                        edge.ConnectedNode = NodeList[i].GetComponent<Node>();

                        if (!tmpList[index].GetComponent<Node>().Connections.Contains(edge))
                            tmpList[index].GetComponent<Node>().Connections.Add(edge);
                    }


                    tmpList.RemoveAt(index);
                }
                else
                {
                    tmpList.RemoveAt(index);
                }
            }
        }

    }

    void DisplayGraphs()
    {
        foreach (GameObject GO in NodeList)
        {
            foreach (Edge edge in GO.GetComponent<Node>().Connections)
            {
                Debug.DrawLine(GO.transform.position, edge.ConnectedNode.gameObject.transform.position, Color.red);
            }
        }
    }



    void OutputToDisk()
    {
        StreamWriter sw;

        string path = "C:\\Users\\u1070737\\Desktop\\AI2\\Map3.txt";

        sw = File.CreateText(path);

        foreach (GameObject GO in NodeList)
        {
            foreach (Edge edge in GO.GetComponent<Node>().Connections)
            {
                sw.WriteLine("(" + GO.GetComponent<Node>().Id.ToString() + "," + edge.ConnectedNode.Id.ToString() + "," + edge.Cost + ")");
            }
        }

        sw.Close();
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






//public class Node
//{
//    public Node()
//    {
////        id = static_id++;
//        m_AdjacentNodeList = new List<WeightNode>();
//    }




//    public int id;
//    public GameObject m_Object;
//    public List<WeightNode> m_AdjacentNodeList;
//}