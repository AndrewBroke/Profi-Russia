using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsController : Modifier
{
    [SerializeField] private float wingsForce = 4;

    private bool isReady = true;

    [SerializeField] private int cooldownTime;

    private void Start()
    {
        InitializeRigidbody();
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
