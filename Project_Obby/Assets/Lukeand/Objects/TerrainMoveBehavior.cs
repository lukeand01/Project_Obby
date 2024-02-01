using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMoveBehavior : MonoBehaviour
{
    [SerializeField] List<Vector3> dirList;
    int currentDirIndex;

    [SerializeField] float totalTimerToStartMoving;
    float currentTimerToStartMoving;
    [SerializeField] float speed;
    [SerializeField] float timeBetweenDir;
    [SerializeField] bool stopAfterListIsDone;

    bool hasStarted;

    public string id {  get; private set; }

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //need to make this character follow.
        if (collision.transform.tag != "Player") return;

        PlayerHandler.instance.MakeParent(this);
       
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag != "Player") return;

        PlayerHandler.instance.CancelParent(this);
    }


    private void Update()
    {
        if (hasStarted) return;

        if(currentTimerToStartMoving > totalTimerToStartMoving)
        {
            hasStarted = true;
            StartCoroutine(TerrainMoveProcess());
        }
        else
        {
            currentTimerToStartMoving += Time.deltaTime;
        }
    }

    IEnumerator TerrainMoveProcess()
    {
        for (int i = 0; i < dirList.Count; i++)
        {
            Vector3 targetPos = transform.position + dirList[i];

            transform.DOMove(targetPos, speed);


            while (transform.position != targetPos)
            {
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(timeBetweenDir);
        }

        if (!stopAfterListIsDone)
        {
            StartCoroutine(TerrainMoveProcess());
        }

    }

}

//i want this 