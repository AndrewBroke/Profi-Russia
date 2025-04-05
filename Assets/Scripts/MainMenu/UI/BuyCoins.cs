using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyCoins : SimpleButton
{
    public BuyCoinsController controller;

    [SerializeField] private int cost;

    public override void OnClick()
    {
        base.OnClick();
        controller.BuyCoins(cost);
    }

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("BuyCoinsController").GetComponent<BuyCoinsController>();
        AddClickListener();
    }
}
