using System.Collections.Generic;
using UnityEngine;
using Watermelon;

public class WallsBehaviour : MonoBehaviour
{
    [SerializeField] List<Renderer> wallRenderers;

    private static WallsBehaviour instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        StoreController.OnProductSelected += OnWallSkinSelected;
    }


    private void OnDisable()
    {
        StoreController.OnProductSelected -= OnWallSkinSelected;
    }


    public static void Init()
    {
        instance.OnWallSkinSelected(StoreController.GetSelectedProduct(StoreProductType.EnvironmentSkin));
    }

    private void OnWallSkinSelected(StoreProduct product)
    {
        EnvironmentSkinProduct envSkin = (EnvironmentSkinProduct)product;

        for (int i = 0; i < wallRenderers.Count; i++)
        {
            wallRenderers[i].material = envSkin.wallMaterial;
        }
    }
}
