using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    
    public void SetUpDestroy(float timer)
    {
        Invoke(nameof(Destroy), timer);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

}
