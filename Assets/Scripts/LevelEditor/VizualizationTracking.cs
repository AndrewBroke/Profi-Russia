using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VizualizationTracking : MonoBehaviour
{
    public GameObject vizualizationObject;

    private void Update()
    {
        if(vizualizationObject != null)
        {
            Vizualize();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateObject();
        }
    }
    public void Vizualize()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "Wall")
            {
                vizualizationObject.transform.position = hit.point;
            }
        }
    }

    public void RotateObject()
    {
        if(vizualizationObject != null)
        {
            vizualizationObject.transform.Rotate(0, 0, 90);
        }
        
    }
}
