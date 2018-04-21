using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public GameObject Map;
    public GameObject AStarObject;
    public GameObject Food;
    public GameObject SnakeBodyObject;
    public GameObject NodeObject;

    List<Node> Path;
    Color NodeColor;


    int step = 1;
    int size = 0;
    float currentTime = 0.0f;

    int NumFoods = 0;
    int snakeSize = 1;

    // Use this for initialization
    void Start ()
    {
        NodeColor = NodeObject.GetComponent<Renderer>().sharedMaterial.color;
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

            if(Path.Count <= 1)
            {
                Debug.Log("End the Game");
                return;
            }

            Transform target = Path[step].transform;


            if (Vector3.Distance(transform.position, Food.transform.position) < 1.1f)
            {
                NumFoods++;

                Debug.Log(snakeSize + NumFoods / 2);

 //               UpdateBody();
                AddBody();

                RaycastHit hit;

                Vector3 newPos = new Vector3(Random.Range(-size, size), 1, Random.Range(-size, size));

                while(Physics.Raycast(newPos + new Vector3(0, 2, 0), Vector3.down, out hit, 5) && hit.transform.gameObject.tag != "Node")
                {
                    newPos = new Vector3(Random.Range(-size, size), 1, Random.Range(-size, size));
                }

                Food.transform.position = newPos;
            }
            else
            {
                Map.GetComponent<AlignGraphsGenerator>().CalculateEdges();
                CalculatePath();
                UpdateBody();
                transform.position = new Vector3(target.position.x, 1, target.position.z);
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

        if(Path != null)
        {
            foreach (Node node in Path)
            {
                node.gameObject.GetComponent<Renderer>().material.color = NodeColor;
            }
        }

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


        if(Path == null || Path.Count <= 1)
        {
            Debug.Log("Can't find a path, looking for a new path");
            StartWandering();
        }

        if(Path != null)
        {
            foreach (Node node in Path)
            {
                node.gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }

    }

    void StartWandering()
    {
        Debug.Log("Wandering...");
        RaycastHit hit;

        GameObject potentialNode = null;
        int biggestSpace = 0;

        if (Physics.Raycast(transform.position + new Vector3(1, 0, 0), Vector3.down, out hit, 1))
        {
            Vector3 dir = new Vector3(1, 0, 0);
            int space = LookingForSpace(transform.position + dir, dir);
            
            if(space > biggestSpace)
            {
                biggestSpace = space;

                if (Physics.Raycast(transform.position + space * dir, Vector3.down, out hit, 1))
                {
                    potentialNode = hit.transform.gameObject;
                }
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(-1, 0, 0), Vector3.down, out hit, 1))
        {
            Vector3 dir = new Vector3(-1, 0, 0);
            int space = LookingForSpace(transform.position + dir, dir);

            if (space > biggestSpace)
            {
                biggestSpace = space;

                if (Physics.Raycast(transform.position + space * dir, Vector3.down, out hit, 1))
                {
                    potentialNode = hit.transform.gameObject;
                }
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 1), Vector3.down, out hit, 1))
        {
            Vector3 dir = new Vector3(0, 0, 1);
            int space = LookingForSpace(transform.position + dir, dir);

            if (space > biggestSpace)
            {
                biggestSpace = space;

                if (Physics.Raycast(transform.position + space * dir, Vector3.down, out hit, 1))
                {
                    potentialNode = hit.transform.gameObject;
                }
            }

        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, -1), Vector3.down, out hit, 1))
        {
            Vector3 dir = new Vector3(0, 0, -1);
            int space = LookingForSpace(transform.position + dir, dir);

            if (space > biggestSpace)
            {
                biggestSpace = space;

                if (Physics.Raycast(transform.position + space * dir, Vector3.down, out hit, 1))
                {
                    potentialNode = hit.transform.gameObject;
                }
            }
        }


        AStarObject.GetComponent<AStar>().EndObj = potentialNode;
        Path = AStarObject.GetComponent<AStar>().GetShortestPathAstart();
    }

    int LookingForSpace(Vector3 startPoint, Vector3 dir)
    {
        RaycastHit hit;
        Vector3 currentPoint = startPoint + new Vector3(0, 1, 0);

        int space = 0;

        while(Physics.Raycast(currentPoint, Vector3.down, out hit, 10))
        {
            if (hit.transform.gameObject.tag != "Node")
                break;

            currentPoint += dir;
            space++;
        }

        return space;
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


        Vector3 newPos = thisbody.GetComponent<SnakeBody>().PrevPosition;
        GameObject newBody = Instantiate(SnakeBodyObject, newPos, Quaternion.identity);
        thisbody.GetComponent<SnakeBody>().next = newBody;

    }


    void UpdateBody()
    {
        GameObject nextbody = gameObject.GetComponent<SnakeBody>().next;

        List<GameObject> list = new List<GameObject>();
        list.Add(gameObject);

        while (nextbody != null)
        {
            list.Add(nextbody);
            nextbody = nextbody.transform.GetComponent<SnakeBody>().next;
        }


        for(int i = list.Count - 1; i >= 0; i--)
        {
            list[i].GetComponent<SnakeBody>().PrevPosition = list[i].transform.position;

            if(i > 0)
                list[i].transform.position = list[i - 1].transform.position;
        }
    }


}
