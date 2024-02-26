using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Store / Graphic")]
public class StoreGraphicData : StoreData
{
    public GraphicType graphicType;


    public override void Buy()
    {
        BaseBuy();

        //and then we give give the skin to the player right away.
        PlayerHandler.instance.graphic.SetGraphicIndex((int)graphicType);

    }

    public override StoreGraphicData GetGraphic() => this;
    
}
