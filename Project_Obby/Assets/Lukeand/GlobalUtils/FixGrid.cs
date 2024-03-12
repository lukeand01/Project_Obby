using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixGrid : MonoBehaviour
{

    //it will give 5 seconds for everything to be put in place.


    [SerializeField] Vector3 itemScale = Vector3.one;

    private void Start()
    {
        Invoke(nameof(UpdateItems), 0.15f);
    }

    void UpdateItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform childRectTransform = transform.GetChild(i) as RectTransform;

            childRectTransform.localScale = itemScale;

            if (childRectTransform != null)
            {
                // Set the Z position to 0
                childRectTransform.localPosition = new Vector3(childRectTransform.localPosition.x, childRectTransform.localPosition.y, 0f);
            }
        }
    }




}
