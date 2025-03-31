using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;

public class EnvironmentSkinProduct : StoreProduct
{
    public Material wallMaterial;
    public bool rotateOnCreate = false;

    public EnvironmentSkinProduct()
    {
        BehaviourType = BehaviourType.Default;
        Type = StoreProductType.EnvironmentSkin;
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Unlock()
    {
        GameController.Coins -= StoreController.Database.GetProductPrice(Type);
    }

    public override bool IsUnlocked()
    {
        return StoreController.IsProductUnlocked(ID);
    }

    public override bool CanBeUnlocked()
    {
        return GameController.Coins >= StoreController.Database.EnvironmentSkinPrice;
    }
}