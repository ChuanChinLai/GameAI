using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    public GameObject GraphsGenerator;
    public int StartNode = 0;
    string Path;


    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Run DijkstraAlgorithm");

            float[,] graph = GraphsGenerator.GetComponent<GraphsGenerator>().graph;
            int NumNodes = GraphsGenerator.GetComponent<GraphsGenerator>().NumNodes;

            DijkstraAlgorithm(graph, NumNodes, StartNode, 100);
        }
	}

    int minDistance(List<float> dist, List<bool> sptSet)
    {
        // Initialize min value
        float min = float.MaxValue;
        int min_index = -1;

        for (int v = 0; v < dist.Count; v++)
        {
            if (sptSet[v] == false && dist[v] <= min)
            {
                min = dist[v];
                min_index = v;
            }
        }

        return min_index;
    }

    void printPath(List<int> parent, int j)
    {
        // Base Case : If j is source
        if (parent[j] == -1)
            return;

        printPath(parent, parent[j]);
        Path = Path + "->" + j;
    }


    void printSolution(List<float> dist, int n, List<int> parent)
    {
        int src = StartNode;

        Debug.Log("Vertex\t  Distance\tPath");

        for (int i = 1; i < n; i++)
        {
            Debug.Log(src + "->" + i + " " + dist[i]);

            Path = src.ToString();

            printPath(parent, i);

            Debug.Log(Path);
        }
    }


    void DijkstraAlgorithm(float[,] graph, int V, int source, int end)
    {
        Debug.Assert(source < V - 1);

        List<float> dist = new List<float>();
        List<bool> sptSet = new List<bool>();
        List<int> parent = new List<int>();

     
        for (int i = 0; i < V; i++)
        {
            parent.Add(-1);
            dist.Add(float.MaxValue);
            sptSet.Add(false);
        }

        dist[source] = 0;


        for(int count = 0; count < V - 1; count++)
        {

            int u = minDistance(dist, sptSet);
            sptSet[u] = true;

            for(int v = 0; v < V; v++)
            {
                if(!sptSet[v] && !graph[u, v].Equals(0) && dist[u] + graph[u, v] < dist[v])
                {
                    parent[v] = u;
                    dist[v] = dist[u] + graph[u, v];
                }
            }
        }

        printSolution(dist, V, parent);
    }




    void HeuristicEstimate()
    {

    }
}
