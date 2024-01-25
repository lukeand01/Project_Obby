using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchFall : MonoBehaviour
{

    //when you touch this it starts shaking anda then starts falling.

    GameObject graphic;
    Rigidbody rb;

    bool processStarted;

    Vector3 originalPos;

    [Tooltip("How many times it shakes till it falls")][SerializeField] float shakingTurns;
    [SerializeField] float intensityOfShake;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
        graphic = transform.GetChild(0).gameObject;
        originalPos = graphic.transform.localPosition;

        rb.useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (processStarted) return;
        if (collision.gameObject.tag != "Player") return;


        StartCoroutine(FallProcess());

    }

    [ContextMenu("DEBUG START FALL PROCESS")]
    public void DEBUGSHOWPROCESS()
    {
        StartCoroutine(FallProcess());
    }


    IEnumerator FallProcess()
    {
        processStarted = true;       
        Debug.Log("started");

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < shakingTurns; i++)
        {
            float x = Random.Range(-intensityOfShake, intensityOfShake);
            float y = Random.Range(-intensityOfShake, intensityOfShake);
            float z = Random.Range(-intensityOfShake, intensityOfShake);
            graphic.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z + z);
            yield return new WaitForSeconds(0.08f);
        }

        Debug.Log("got here");
        rb.useGravity = true;
    }

}
