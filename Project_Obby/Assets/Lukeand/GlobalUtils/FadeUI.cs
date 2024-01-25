using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FadeUI : MonoBehaviour
{
    //this is any ui that i want to fade overtime.
    [SerializeField]TextMeshProUGUI text;
    [SerializeField] AnimationCurve alphaColorCurve;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] AnimationCurve heightCurve;
    float time;


    Vector3 origin;
    Vector3 originalScale;
    Camera cam;

    bool isReusable;

    private void Start()
    {
        transform.localScale = new Vector3(1,1,1);
        origin = transform.position;
        originalScale = transform.localScale;
        cam = Camera.main;

    }


    private void Update()
    {
        if (text == null) return;

        text.color = new Color(text.color.r, text.color.g, text.color.b, alphaColorCurve.Evaluate(time));
        transform.localScale = originalScale * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, 0 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;

        if(text.color.a <= 0)
        {
            if (isReusable) gameObject.SetActive(false);
            else Destroy(gameObject);
        }

    }

    public void Reuse()
    {
        isReusable = true;
    }

    public void SetUp(string text, Color color, bool crit = false)
    {
        //then we change a bit the vector.
        //the problem is that i dont want the fellas to overshadow each other.       
        this.text.text = text;
        this.text.color = color;
        time = 0;
       
    }
}
