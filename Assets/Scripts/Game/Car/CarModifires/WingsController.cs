using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsController : MonoBehaviour
{
    [SerializeField] private CarMovement carMovement;

    [SerializeField] private float wingsForce = 4;

    private Rigidbody rbCar;

    private bool isReady = true;

    [SerializeField] private int cooldownTime;

    private void Start()
    {
        rbCar = carMovement.GetComponent<Rigidbody>();
    }

    public void ActivateWings()
    {
        if (!isReady) return;
        rbCar.AddForce(carMovement.transform.up * wingsForce, ForceMode.Impulse);
        isReady = false;
        StartCoroutine("Cooldown");
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        isReady = true;
    }
}
