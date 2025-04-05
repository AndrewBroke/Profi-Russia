using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailMoving : MonoBehaviour
{
    public GameObject currentDetail;

    public void Move()
    {
        if(currentDetail.GetComponent<SelectDetail>().isCanMoving == true)
        {
            Vector3 mousePos = Input.mousePosition;
            currentDetail.transform.position = mousePos;
        }
    }
}
