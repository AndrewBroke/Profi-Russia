using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunController : MonoBehaviour
{
    [SerializeField] private CarMovement carMovement;

    [SerializeField] private float fanForce = 7;

    private Rigidbody rbCar;

    private bool isUsing;

    private void Start()
    {
        rbCar = carMovement.GetComponent<Rigidbody>();
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
