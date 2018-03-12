using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class UnitTest : MonoBehaviour
{
    public int NumTest = 50;

    float time;
    public GameObject DijkstraObject;
    public GameObject AStarObject;


    public int      TotalNodeVisits = 0;
    public float    TotalRunningTime = 0;
    public double   TotalPathCost = 0;


    // Use this for initialization
    void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ResetTest();

            StreamWriter streamWriter;
            string rootPath = "C:\\Users\\u1070737\\Desktop\\AI2\\" + "D1.txt";
            streamWriter = File.CreateText(rootPath);
            streamWriter.WriteLine("Running Dijkstra Algorithm 50 Times...");

            for(int i = 0; i < NumTest; i++)
            {
                TestDijkstra();
                Debug.Log("Progress: " + i.ToString());
            }

            streamWriter.WriteLine(TotalNodeVisits / NumTest);
            streamWriter.WriteLine(TotalRunningTime / NumTest);
            streamWriter.WriteLine(TotalPathCost / NumTest);

            streamWriter.Close();
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Start Test");
            ResetTest();

            StreamWriter streamWriter;
            string rootPath = "C:\\Users\\u1070737\\Desktop\\AI2\\" + "A1.txt";
            streamWriter = File.CreateText(rootPath);
            streamWriter.WriteLine("Running Astar Algorithm 50 Times...");

            for (int i = 0; i < NumTest; i++)
            {
                TestAstar();
                Debug.Log("Progress: " + i.ToString());
            }

            streamWriter.WriteLine(TotalNodeVisits / NumTest);
            streamWriter.WriteLine(TotalRunningTime / NumTest);
            streamWriter.WriteLine(TotalPathCost / NumTest);

            streamWriter.Close();
        }

    }


    void TestDijkstra()
    {
        DijkstraObject.GetComponent<Dijkstra>().StartObj = GameObject.Find("Node" + Random.Range(0, 16000));
        DijkstraObject.GetComponent<Dijkstra>().EndObj   = GameObject.Find("Node" + Random.Range(0, 16000));

        var res = DijkstraObject.GetComponent<Dijkstra>().GetShortestPathDijikstra();

        Debug.Log(DijkstraObject.GetComponent<Dijkstra>().NodeVisits);
        Debug.Log(DijkstraObject.GetComponent<Dijkstra>().RunningTime);
        Debug.Log(DijkstraObject.GetComponent<Dijkstra>().PathCost);

        TotalNodeVisits += DijkstraObject.GetComponent<Dijkstra>().NodeVisits;
        TotalRunningTime += DijkstraObject.GetComponent<Dijkstra>().RunningTime;
        TotalPathCost += DijkstraObject.GetComponent<Dijkstra>().PathCost;


    }


    void TestAstar()
    {
        AStarObject.GetComponent<AStar>().StartObj = GameObject.Find("Node" + Random.Range(0, 16000));
        AStarObject.GetComponent<AStar>().EndObj = GameObject.Find("Node" + Random.Range(0, 16000));

        var res = AStarObject.GetComponent<AStar>().GetShortestPathAstart();

        Debug.Log(AStarObject.GetComponent<AStar>().NodeVisits);
        Debug.Log(AStarObject.GetComponent<AStar>().RunningTime);
        Debug.Log(AStarObject.GetComponent<AStar>().PathCost);

        TotalNodeVisits += AStarObject.GetComponent<AStar>().NodeVisits;
        TotalRunningTime += AStarObject.GetComponent<AStar>().RunningTime;
        TotalPathCost += AStarObject.GetComponent<AStar>().PathCost;
    }


    private void ResetTest()
    {
        TotalNodeVisits = 0;
        TotalRunningTime = 0;
        TotalPathCost = 0;
    }
}
