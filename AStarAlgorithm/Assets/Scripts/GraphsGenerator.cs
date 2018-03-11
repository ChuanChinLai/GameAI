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


    private int nodeID = 0;

    // Use this for initialization
    void Start ()
    {
        graph = new float[NumNodes, NumNodes];

        CreateGraphs();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayGraphs();


        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Output Node");

            OutputToDisk();
        }
    }


    void CreateGraphs()
    {
        for(int i = 0; i < NumNodes; i++)
        {

            Vector3 pos = new Vector3(Random.Range(-BoxSize, BoxSize), Random.Range(-BoxSize, BoxSize), Random.Range(-BoxSize, BoxSize));
            GameObject GO = Instantiate(NodeGameObject, pos, Quaternion.identity);
            GO.name = "Node" + nodeID;
            GO.GetComponent<Node>().id = nodeID++;
            NodeList.Add(GO);
        }


        for (int i = 0; i < NodeList.Count; i++)
        {
            Debug.Assert(NumNodes > MaxNumAdjacentEdges);

            List<GameObject> tmpList = new List<GameObject>(NodeList);
            tmpList.RemoveAt(i);

            int NumAdjacentEdges = Random.Range(1, MaxNumAdjacentEdges);

            while (NodeList[i].GetComponent<Node>().m_AdjacentNodeList.Count < NumAdjacentEdges && tmpList.Count != 0)
            {
                int index = Random.Range(0, tmpList.Count);

                float dis = Vector3.Distance(NodeList[i].transform.position, tmpList[index].transform.position);

                if (dis < MaxDistanceToBeNeighbor && !NodeList[i].GetComponent<Node>().m_AdjacentNodeList.ContainsKey(tmpList[index]))
                {
                    NodeList[i].GetComponent<Node>().m_AdjacentNodeList.Add(tmpList[index], dis);
                    tmpList[index].GetComponent<Node>().m_AdjacentNodeList.Add(NodeList[i], dis);

                    graph[i, tmpList[index].GetComponent<Node>().id] = dis;
                    graph[tmpList[index].GetComponent<Node>().id, i] = dis;

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
            foreach (KeyValuePair<GameObject, float> node in GO.GetComponent<Node>().m_AdjacentNodeList)
            {
                Debug.DrawLine(GO.transform.position, node.Key.transform.position, Color.red);
            }
        }
    }



    void OutputToDisk()
    {
        StreamWriter sw;

        string path = "C:\\Users\\u1070737\\Desktop\\Test2.txt";

        sw = File.CreateText(path);

        foreach (GameObject GO in NodeList)
        {
            foreach (KeyValuePair<GameObject, float> node in GO.GetComponent<Node>().m_AdjacentNodeList)
            {
                sw.WriteLine("(" + GO.GetComponent<Node>().id.ToString() + "," + node.Key.GetComponent<Node>().id.ToString() + "," + node.Value + ")");
            }
        }

        sw.Close();
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