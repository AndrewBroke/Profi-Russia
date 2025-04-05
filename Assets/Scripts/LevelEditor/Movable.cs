using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public GameObject finish;

    public void AcivateFinish()
    {
        finish.SetActive(true);
    }
}
