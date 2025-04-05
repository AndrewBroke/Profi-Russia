using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyCoinsController : MonoBehaviour
{
    [SerializeField] private CoinsData coinsData;

    [SerializeField] private GameObject purchaseMenu;

    [SerializeField] private PlayerMoneyController playerMoneyController;
    private int coins;
    private bool isPurchase;
    public void BuyCoins(int count)
    {
        StartCoroutine("Purchase");
        coins = count;
    }
    IEnumerator Purchase()
    {
        purchaseMenu.SetActive(true);
        yield return new WaitForSeconds(3);
        purchaseMenu.SetActive(false);
        coinsData.coins += coins;
        playerMoneyController.PrintCoins();
    }
}
