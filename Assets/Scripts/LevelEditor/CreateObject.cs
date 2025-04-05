using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateObject : MonoBehaviour
{
    [SerializeField] private VizualizationTracking vizualizationTracking;

    [SerializeField] private GameObject[] objects;

    [SerializeField] private CreatedObjects createdObjects;

    private void Update()
    {
        if(vizualizationTracking.vizualizationObject != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Create();
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    CancelCreate();
                }
            }
        }
        
    }
    public void Create()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        for(int i = 0; i < objects.Length; i++)
        {
            if(vizualizationTracking.vizualizationObject.name == objects[i].name)
            {
                GameObject create = Instantiate(objects[i], vizualizationTracking.vizualizationObject.transform.position, vizualizationTracking.vizualizationObject.transform.rotation);
                createdObjects.objects.Add(create);
            }
        }
    }
    public void CancelCreate()
    {
        if(createdObjects.objects.Count > 0)
        {
            Destroy(createdObjects.objects[createdObjects.objects.Count -1]);
            createdObjects.objects.RemoveAt(createdObjects.objects.Count - 1);
            print(createdObjects.objects.Count);
        }
    }
}
