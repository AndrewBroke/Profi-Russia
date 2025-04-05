using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    [SerializeField] private VizualizationTracking vizualizationTracking;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    public void Select(GameObject selectedObject)
    {

        if(vizualizationTracking.vizualizationObject != null) vizualizationTracking.vizualizationObject.SetActive(false);
        vizualizationTracking.vizualizationObject = selectedObject;
        virtualCamera.Follow = vizualizationTracking.vizualizationObject.transform;
        vizualizationTracking.vizualizationObject.SetActive(true);
    }
}
