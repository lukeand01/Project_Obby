using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixZPositionForGrid : MonoBehaviour
{

    private GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        // Get the GridLayoutGroup component
        gridLayoutGroup = GetComponent<GridLayoutGroup>();


    }

    void Update()
    {
        // Check if the children of the layout group have changed


        AdjustChildrenZPosition();

       
    }

    void AdjustChildrenZPosition()
    {
        // Iterate through the child images
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform childRectTransform = transform.GetChild(i) as RectTransform;
            if (childRectTransform != null)
            {
                // Set the Z position to 0
                childRectTransform.localPosition = new Vector3(childRectTransform.localPosition.x, childRectTransform.localPosition.y, 0f);
            }
        }
    }
}

