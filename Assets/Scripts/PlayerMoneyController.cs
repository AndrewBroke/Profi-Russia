using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoneyController : MonoBehaviour
{
    [SerializeField] private CoinsData coinsData;
    [SerializeField] private Text cointCountText;

    private void Start()
    {
        PrintCoins();
    }
    public void ChangeData(int value)
    {
        if(coinsData.coins + value >= 0)
        {
            coinsData.coins += value;
            PrintCoins();
        }
    }
    public void BuyCoins()
    {

    }
    public void PrintCoins()
    {
        if(coinsData.coins >= 1000)
        {
            float value = coinsData.coins / 1000.0f;
            cointCountText.text = $"{value}ê";
        }
        else
        {
            cointCountText.text = coinsData.coins.ToString();
        }
    }
}
