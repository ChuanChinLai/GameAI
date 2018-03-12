using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public GameObject Map;
    public GameObject AStarObject;
    public GameObject Food;
    public GameObject SnakeBodyObject;

    List<Node> Path;

    int step = 1;
    int size = 0;
    float currentTime = 0.0f;


    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CalculatePath();
            size = Map.GetComponent<AlignGraphsGenerator>().NumNodesInEdge;
        }


        if(Path != null && Path.Count != 0)
        {
            currentTime = 0;
            Transform target = Path[step].transform;

            if (Vector3.Distance(transform.position, target.position) < 0.001f)
            {
                step++;

                if(step >= Path.Count)
                {
                    Debug.Log("EAT FOOD");

                    AddBody();

                    Food.transform.position = new Vector3(Random.Range(-size, size), 1, Random.Range(-size, size));
                    CalculatePath();
                    step = 0;
                }
            }
            else
            {
                UpdateBody();
                transform.position = target.position;
            }
        }
        else
        {
            currentTime += Time.deltaTime;
        }
        

    }

    void CalculatePath()
    {
        RaycastHit hit;

        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
            {
                AStarObject.GetComponent<AStar>().StartObj = hit.transform.gameObject;
            }
            else if(Path != null)
            {
                AStarObject.GetComponent<AStar>().StartObj = Path[step - 1].gameObject;
            }


            if (Physics.Raycast(Food.transform.position, Vector3.down, out hit, 1))
            {
                AStarObject.GetComponent<AStar>().EndObj = hit.transform.gameObject;
            }

        }

        Path = AStarObject.GetComponent<AStar>().GetShortestPathAstart();
    }


    void AddBody()
    {
        GameObject thisbody = gameObject.GetComponent<SnakeBody>().gameObject;
        GameObject nextbody = gameObject.GetComponent<SnakeBody>().next;

        while (nextbody != null)
        {
            thisbody = nextbody;
            nextbody = nextbody.transform.GetComponent<SnakeBody>().next;
        }


        GameObject newBody = Instantiate(SnakeBodyObject, thisbody.transform.position, Quaternion.identity);
        thisbody.GetComponent<SnakeBody>().next = newBody;
    }


    void UpdateBody()
    {
        GameObject thisbody = gameObject.GetComponent<SnakeBody>().gameObject;
        GameObject nextbody = gameObject.GetComponent<SnakeBody>().next;

        List<GameObject> list = new List<GameObject>();
        list.Add(gameObject);

        while (nextbody != null)
        {
            list.Add(nextbody);
            nextbody = nextbody.transform.GetComponent<SnakeBody>().next;
        }


        for(int i = list.Count - 1; i > 0; i--)
        {
            list[i].transform.position = list[i - 1].transform.position;
        }
    }


}
