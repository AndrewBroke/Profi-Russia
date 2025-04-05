using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void ActivateRocket()
    {
        rb.isKinematic = false;
        rb.velocity = new Vector3(Vector3.forward.z, 0, 0) * speed;
    }
}
