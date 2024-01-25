using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageClass 
{

    public float baseDamage { get; private set; }
    float critChance;
    float critDamage;
    float damageBasedInHealth;

    //we get the health scaling.


    public bool cannotFinishEntity { get; private set;}


    public DamageClass(float baseDamage)
    {
        this.baseDamage = baseDamage;       
    }
    

    #region MAKE

    public void MakeBlockFromFinishingEntity()
    {
        cannotFinishEntity = true;
    }

    public void MakeCritChance(float critChance)
    {
        this.critChance = critChance;
    }

   

    #endregion


    

    public float GetDamage()
    {
        return baseDamage;
    }
}
