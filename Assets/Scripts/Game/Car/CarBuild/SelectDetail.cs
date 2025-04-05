using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDetail : MonoBehaviour
{
    public DetailMoving detailMoving;
    public bool isCanMoving;
    private void Start()
    {
        detailMoving = GameObject.FindGameObjectWithTag("MoveDetail").GetComponent<DetailMoving>();
    }
    public void Select()
    {
        if(isCanMoving == true)
        {
            detailMoving.currentDetail = gameObject;
        }
    }
}
