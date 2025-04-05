using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonController : SimpleButton
{

    private CarMovement carMovement;
    public override void OnClick()
    {
        base.OnClick();
        carMovement.isStarted = true;
        gameObject.SetActive(false);

    }

    private void Start()
    {
        carMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CarMovement>();
        AddClickListener();
    }
}
