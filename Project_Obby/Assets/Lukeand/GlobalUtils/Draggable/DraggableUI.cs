using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableUI : MonoBehaviour
{
    //it receies an object and sendsd object. the receivers are responsible for translating to what they want.
    [SerializeField] Vector2 forceSize = Vector2.one;
    [SerializeField] Image draggingImage;

    object data;
    //need to find a way for this to interact with this fella.


    private void Awake()
    {
        draggingImage.gameObject.SetActive(false);
    }


    private void FixedUpdate()
    {
        if (data == null) return;
        if (Input.touchCount <= 0) return;

        Touch touch = Input.GetTouch(0);
        Vector2 touchPos = touch.position;
        draggingImage.transform.position = touchPos;

        //need a way to cast from here into someone wiwth idraggable interface.
        //or make the unit ask the handler forr information.
    }


    public void ReceiveData(object data, Sprite dataSprite)
    {
        this.data = data;
        draggingImage.gameObject.SetActive(true);
        draggingImage.sprite = dataSprite;
    }

    public void ClearData()
    {
        data = null;
        draggingImage.gameObject.SetActive(false);

    }

    public object GetData()
    {
        return data;
    }
}
