using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GraphsGenerator : MonoBehaviour
{
    public GameObject NodeGameObject;

    public int NumNodes = 5;
    public int MaxNumAdjacentEdges = 10;

    public float BoxSize = 10.0f;
    public float MaxDistanceToBeNeighbor = 10.0f;


    List<Node> NodeList = new List<Node>();

    // Use this for initialization
    void Start ()
    {
        CreateGraphs();

    }
	
	// Update is called once per frame
	void Update ()
    {
        DisplayGraphs();
    }



    void CreateGraphs()
    {
        for(int i = 0; i < NumNodes; i++)
        {
            Node node = new Node();

            Vector3 pos = new Vector3(Random.Range(-BoxSize, BoxSize), Random.Range(-BoxSize, BoxSize), Random.Range(-BoxSize, BoxSize));
            GameObject GO = Instantiate(NodeGameObject, pos, Quaternion.identity);
            node.m_Object = GO;

            NodeList.Add(node);
        }


        for (int i = 0; i < NodeList.Count; i++)
        {
            Debug.Assert(NumNodes > MaxNumAdjacentEdges);

            List<Node> tmpList = new List<Node>(NodeList);
            tmpList.RemoveAt(i);

            int NumAdjacentEdges = Random.Range(1, MaxNumAdjacentEdges);

            while (NodeList[i].m_AdjacentNodeList.Count < NumAdjacentEdges && tmpList.Count != 0)
            {
                int index = Random.Range(0, tmpList.Count);

                float dis = Vector3.Distance(NodeList[i].m_Object.transform.position, tmpList[index].m_Object.transform.position);

                if (dis < MaxDistanceToBeNeighbor)
                {
                    WeightNode ToWN = new WeightNode();
                    ToWN.node = tmpList[index];
                    ToWN.weight = dis;
                    NodeList[i].m_AdjacentNodeList.Add(ToWN);


                    WeightNode FromWN = new WeightNode();
                    FromWN.node = NodeList[i];
                    FromWN.weight = dis;
                    tmpList[index].m_AdjacentNodeList.Add(FromWN);

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
        foreach (Node node in NodeList)
        {
            foreach (WeightNode wn in node.m_AdjacentNodeList)
            {
                Debug.DrawLine(node.m_Object.transform.position, wn.node.m_Object.transform.position, Color.red);
            }
        }
    }


    struct WeightNode
    {
        public Node node;
        public float weight;
    }


    class Node
    {
        public Node()
        {
            id = static_id++;
            m_AdjacentNodeList = new List<WeightNode>();
        }


        static int static_id = 0;

        public int id;
        public GameObject m_Object;
        public List<WeightNode> m_AdjacentNodeList;
    }

}
