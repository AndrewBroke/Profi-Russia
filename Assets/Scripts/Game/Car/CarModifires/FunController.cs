using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunController : Modifier
{

    [SerializeField] private float fanForce = 7;

    private bool isUsing;

    private void Start()
    {
        InitializeRigidbody();
    }

    private void Update()
    {
        if (!isUsing) return;
        rbCar.AddForce(carMovement.transform.forward * fanForce, ForceMode.Acceleration);
    }

    public void SetFanActive(bool value)
    {
        isUsing = value;
    }
}
