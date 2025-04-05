using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier : MonoBehaviour
{
    public CarMovement carMovement;
    public Rigidbody rbCar;

    public void InitializeRigidbody()
    {
        rbCar = carMovement.GetComponent<Rigidbody>();
    }
}
