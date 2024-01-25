using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyGrid : LayoutGroup
{
    //need to create limitations.


    [Separator("SIZE")]
    [SerializeField] bool canControlSize;
    [ConditionalField(nameof(canControlSize))][SerializeField] Vector3 size = Vector3.one;

    [Separator("SPACING")]
    [SerializeField] float spacing;
    [SerializeField] float spacingY;

    [Separator("Orientation")]
    [SerializeField] GridOrientation gridOrientation;

    [Separator("Limit")]
    public int limitPerLine = 3 ;

    enum GridOrientation
    {
        Middle,
        Right,
        Left
    }

    RectTransform container;

    public override void CalculateLayoutInputVertical()
    {

        container = GetComponent<RectTransform>();

        HandleGrid();
    }

    void HandleGrid()
    {
        float width = container.sizeDelta.x;
        float height = container.sizeDelta.y;

        int currentLimit = 0;

        int posX = 0;
        int posY = 0;
        for (int i = 0; i < rectChildren.Count; i++)
        {
            //ok so i need the height and the widht of the container.
            var item = rectChildren[i];
            float itemWidth = item.sizeDelta.x;
            currentLimit++;

            if(currentLimit > limitPerLine)
            {
                currentLimit = 1;
                posY += (int)(height + spacingY - 100);
                posX = 0;
            }



            if (canControlSize)
            {
                item.localScale = size;
            }


            

            SetChildAlongAxis(item, 0, posX + padding.left + padding.right);
            SetChildAlongAxis(item, 1, posY + padding.top + padding.bottom); //placing in the y axis.
            posX += (int)(itemWidth + spacing);
        }
    }







    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }
}
