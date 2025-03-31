using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;

public class CommingSoonProduct : StoreProduct
{
    public CommingSoonProduct()
    {
        BehaviourType = BehaviourType.Dummy;
    }

    public override void Init()
    {

    }

    public override void Unlock()
    {
        
    }

    public override bool IsUnlocked()
    {
        return true;
    }

    public override bool CanBeUnlocked()
    {
        return false;
    }
}