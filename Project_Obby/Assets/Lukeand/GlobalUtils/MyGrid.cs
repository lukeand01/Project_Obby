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



    [Separator("DIFFERENT TYPES")]
    [SerializeField] bool stackFromCenter;

    [Separator("Limit")]
    public int limitPerLine = 3 ;

    

    RectTransform container;

    public override void CalculateLayoutInputVertical()
    {

        container = GetComponent<RectTransform>();

        if (stackFromCenter)
        {
            HandleGridStackFromCenter();
        }
        else
        {
            HandleGrid();
        }


        //i want the things to always spread from the center.
        
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

    void HandleGridStackFromCenter()
    {

        int posX = 0;

        //then we use spacing.

        int sideModifier = 1;


        for (int i = 0; i < rectChildren.Count; i++)
        {
            //everytime we get the first one and put in the middle, then right
            var item = rectChildren[i];
            float itemWidth = item.sizeDelta.x;

            if (canControlSize)
            {
                item.localScale = size;
            }

            if(i == 0)
            {
                //this does nothing. always in the middle.
                
            }
            else
            {
                if (i % 2 == 0)
                {
                    //par
                    sideModifier = 1;
                }
                else
                {
                    //impar
                    sideModifier = -1;
                }
            }

            SetChildAlongAxis(item, 0, sideModifier * (posX + padding.left + padding.right));
            SetChildAlongAxis(item, 1, padding.top);
            posX += (int)(itemWidth + spacing);

        }
    }


    //i want to create a logic that always tries to keep the objects in the middle.



    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }
}
