using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailCheck : MonoBehaviour
{
    private CarDetails carDetails;

    [SerializeField] private string detailTag;
    private void Start()
    {
        carDetails = GameObject.FindGameObjectWithTag("DetailsList").GetComponent<CarDetails>();
    }   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (detailTag != collision.gameObject.tag) return;
        switch (collision.gameObject.tag)
        {
            case "Fan":
                carDetails.fun.SetActive(true);
                collision.gameObject.GetComponent<SelectDetail>().isCanMoving = false;
                break;
            case "Wings":
                carDetails.wings.SetActive(true);
                collision.gameObject.GetComponent<SelectDetail>().isCanMoving = false;
                break;
            case "Wheel":
                carDetails.wheels.SetActive(true);
                collision.gameObject.GetComponent<SelectDetail>().isCanMoving = false;
                break;
            case "SecondWheel":
                carDetails.secondWheels.SetActive(true);
                collision.gameObject.GetComponent<SelectDetail>().isCanMoving = false;
                break;
            case "SpikesWheel":
                carDetails.spikesWheels.SetActive(true);
                collision.gameObject.GetComponent<SelectDetail>().isCanMoving = false;
                break;
            case "Rocket":
                carDetails.rocket.SetActive(true);
                collision.gameObject.GetComponent<SelectDetail>().isCanMoving = false;
                break;
        }
        
    }
}
