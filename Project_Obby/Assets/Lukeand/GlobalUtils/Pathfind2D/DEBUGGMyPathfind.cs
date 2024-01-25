using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class DEBUGGMyPathfind : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] GameObject template;
    [SerializeField] bool slowProcedure = false;
    Transform container;
    public List<MyNode> path = new();
    private void Awake()
    {
        if(start == null || end == null)
        {
            Destroy(gameObject);
            return;
        }

        container = new GameObject().transform;
        container.transform.parent = transform;
           
    }

    private void Start()
    {
        List<MyNode> pathList = GetComponent<MyPathfind>().GetPathThroughVector(start.position, end.position);
        path = pathList;
        if (slowProcedure) StartCoroutine(PathingProcess(pathList));
        else ForcePathing(pathList);
    }

    IEnumerator PathingProcess(List<MyNode> pathList)
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < pathList.Count; i++)
        {
            GameObject newObject = Instantiate(template, pathList[i].pos, Quaternion.identity);
            yield return new WaitForSeconds(1.5f);
        }

    }

    //the problem is that it comes from the first 
    void ForcePathing(List<MyNode> pathList)
    {
        for (int i = 0; i < pathList.Count; i++)
        {         
            if (i == 0)
            {
                Debug.Log("painted it blue");
                GameObject firstObject = Instantiate(template, pathList[i].cameFrom.transform.position, Quaternion.identity);
                firstObject.name = "First";
                firstObject.transform.parent = container.transform;
                firstObject.GetComponent<SpriteRenderer>().color = Color.blue;
            }

            GameObject newObject = Instantiate(template, pathList[i].pos, Quaternion.identity);
            newObject.transform.parent = container.transform;
            if (i + 1 >= pathList.Count)
            {
                Debug.Log("painted red");
                newObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            
        }



    }

    //then it will turn the last 

}
