using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckMoney : MonoBehaviour
{
    [SerializeField] private int price;

    [SerializeField] private CoinsData coinsData;

    private void Update()
    {
        if(coinsData.coins >= price)
        {
            gameObject.GetComponent<Text>().color = Color.white;
        }
        else
        {
            gameObject.GetComponent<Text>().color = Color.red;
        }
    }
}
