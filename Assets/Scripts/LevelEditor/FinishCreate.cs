using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCreate : MonoBehaviour
{
    [SerializeField] private CreatedObjects createdObjects;
    
    public void CreateFinish()
    {
        print("Вызвана");
        for(int i = createdObjects.objects.Count - 1; i >= 0; i--)
        {
            print(i);
            print(createdObjects.objects[i].name);
            print(createdObjects.objects.Count);
            if (createdObjects.objects[i].tag == "Movable")
            {
                print("есть");
                createdObjects.objects[i].GetComponent<Movable>().AcivateFinish();
                break;
            }
        }
    }
}
