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
                GO.GetComponent<Node>().id = nodeID++;
                NodeList.Add(GO);
            }
        }

        for (int i = 0; i < NodeList.Count; i++)
        {
            RaycastHit hit;

            {
                Vector3 fwd = NodeList[i].transform.TransformDirection(Vector3.forward);

                if (Physics.Raycast(NodeList[i].transform.position, fwd, out hit, 1))
                {
                    if (!NodeList[i].GetComponent<Node>().m_AdjacentNodeList.ContainsKey(hit.transform.gameObject))
                    {
                        NodeList[i].GetComponent<Node>().m_AdjacentNodeList.Add(hit.transform.gameObject, 1);
                        hit.transform.gameObject.GetComponent<Node>().m_AdjacentNodeList.Add(NodeList[i], 1);

                        graph[i, hit.transform.gameObject.GetComponent<Node>().id] = 1;
                        graph[hit.transform.gameObject.GetComponent<Node>().id, i] = 1;
                    }
                }
            }

            {
                Vector3 back = NodeList[i].transform.TransformDirection(Vector3.back);

                if (Physics.Raycast(NodeList[i].transform.position, back, out hit, 1))
                {
                    if (!NodeList[i].GetComponent<Node>().m_AdjacentNodeList.ContainsKey(hit.transform.gameObject))
                    {
                        NodeList[i].GetComponent<Node>().m_AdjacentNodeList.Add(hit.transform.gameObject, 1);
                        hit.transform.gameObject.GetComponent<Node>().m_AdjacentNodeList.Add(NodeList[i], 1);

                        graph[i, hit.transform.gameObject.GetComponent<Node>().id] = 1;
                        graph[hit.transform.gameObject.GetComponent<Node>().id, i] = 1;
                    }
                }
            }

            {
                Vector3 right = NodeList[i].transform.TransformDirection(Vector3.right);

                if (Physics.Raycast(NodeList[i].transform.position, right, out hit, 1))
                {
                    if (!NodeList[i].GetComponent<Node>().m_AdjacentNodeList.ContainsKey(hit.transform.gameObject))
                    {
                        NodeList[i].GetComponent<Node>().m_AdjacentNodeList.Add(hit.transform.gameObject, 1);
                        hit.transform.gameObject.GetComponent<Node>().m_AdjacentNodeList.Add(NodeList[i], 1);

                        graph[i, hit.transform.gameObject.GetComponent<Node>().id] = 1;
                        graph[hit.transform.gameObject.GetComponent<Node>().id, i] = 1;
                    }
                }
            }

            {
                Vector3 left = NodeList[i].transform.TransformDirection(Vector3.left);

                if (Physics.Raycast(NodeList[i].transform.position, left, out hit, 1))
                {
                    if (!NodeList[i].GetComponent<Node>().m_AdjacentNodeList.ContainsKey(hit.transform.gameObject))
                    {
                        NodeList[i].GetComponent<Node>().m_AdjacentNodeList.Add(hit.transform.gameObject, 1);
                        hit.transform.gameObject.GetComponent<Node>().m_AdjacentNodeList.Add(NodeList[i], 1);

                        graph[i, hit.transform.gameObject.GetComponent<Node>().id] = 1;
                        graph[hit.transform.gameObject.GetComponent<Node>().id, i] = 1;
                    }
                }
            }
        }
    }
}