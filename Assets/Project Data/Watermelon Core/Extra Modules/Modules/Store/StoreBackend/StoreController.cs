﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    //Store Module v0.9.2
    [DefaultExecutionOrder(-10)]
    [HelpURL("https://docs.google.com/document/d/1SS9_U59ACe1kSrd2nxSmbqvWtTu6Uv1SQzcyocQLZB8/edit")]
    public class StoreController : MonoBehaviour
    {
        private static StoreController instance;
        public static bool IsInitialized { get; private set; }

        [SerializeField] StoreDatabase storeDatabase;
        public static StoreDatabase Database => instance.storeDatabase;

        public static readonly string SAVE_FILE_NAME = "StoreData.dat";
        public delegate void StoreCallBack(StoreProduct product);
        public static event StoreCallBack OnProductSelected;

        private static StoreSaveData savedData;

        private static int SelectedEnvironmentSkinId => savedData.selectedEnvironmentSkinId;

        private void Awake()
        {
            instance = this;

            storeDatabase.Init();
            IsInitialized = true;
            LoadSave();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
                Save();
        }

        private void OnDestroy()
        {
            Save();
        }

        public static int GetSelectedProductSkinID(StoreProductType productType)
        {
            if (productType == StoreProductType.EnvironmentSkin)
            {
                return SelectedEnvironmentSkinId;
            }
            else
            {
                return -1;
            }
        }

        private static void SetSelectedProductSkinID(StoreProductType productType, int id)
        {
            if (productType == StoreProductType.EnvironmentSkin)
            {
                savedData.selectedEnvironmentSkinId = id;
            }
        }


        public static T GetProduct<T>(int id) where T : StoreProduct
        {
            int productIndex = Array.FindIndex(instance.storeDatabase.Products, x => x.ID == id);

            if (productIndex != -1)
            {
                return (T)instance.storeDatabase.Products[productIndex];
            }

            Debug.LogError("Product with id: " + id + " is not found.");
            return (T)instance.storeDatabase.Products[0];
        }

        public static StoreProduct GetProduct(int id)
        {
            int productIndex = Array.FindIndex(instance.storeDatabase.Products, x => x.ID == id);

            if (productIndex != -1)
            {
                return instance.storeDatabase.Products[productIndex];
            }

            Debug.LogError("Product with id: " + id + " is not found.");
            return instance.storeDatabase.Products[0];
        }

        public static T GetSelectedProduct<T>(StoreProductType type) where T : StoreProduct
        {
            return GetProduct<T>(GetSelectedProductSkinID(type));
        }

        public static StoreProduct GetSelectedProduct(StoreProductType type)
        {
            return GetProduct(GetSelectedProductSkinID(type));
        }

        public static bool TryToBuyProduct(int productId)
        {
            return TryToBuyProduct(GetProduct(productId));
        }

        public static bool TryToBuyProduct(StoreProduct product)
        {
            if (product.CanBeUnlocked())
            {
                product.Unlock();

                savedData.UnlockProduct(product.ID);
                TryToSelectProduct(product.ID);

                instance.Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void UnlockProduct(StoreProduct product)
        {
            UnlockProduct(product.ID);
        }

        public static void UnlockProduct(int productId)
        {
            savedData.UnlockProduct(productId);
            TryToSelectProduct(productId);

            instance.Save();
        }


        public static bool IsProductUnlocked(int id)
        {
            return savedData.IsProductUnlocked(id);
        }

        public static bool IsProductUnlocked(StoreProduct product)
        {
            return IsProductUnlocked(product.ID);
        }

        public static bool TryToSelectProduct(StoreProduct product)
        {
            if (IsProductUnlocked(product.ID) && GetSelectedProductSkinID(product.Type) != product.ID)
            {
                SetSelectedProductSkinID(product.Type, product.ID);
                //StoreUIController.OnNewSkinSelected();
                OnProductSelected?.Invoke(product);

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryToSelectProduct(int id)
        {
            return TryToSelectProduct(GetProduct(id));
        }

        private void Save()
        {
            Serializer.SerializeToPDP(savedData, SAVE_FILE_NAME);
        }

        private void LoadSave()
        {
            savedData = Serializer.DeserializeFromPDP<StoreSaveData>(SAVE_FILE_NAME, Serializer.SerializeType.Binary, "", false);

            if (savedData.unlockedProductsIds == null || savedData.unlockedProductsIds.Count == 0)
            {
                Debug.Log("[Store Module] Default skins initialization.");
                List<int> defaultItemsIds = storeDatabase.GetDefaultSkinsIDs();

                for (int i = 0; i < defaultItemsIds.Count; i++)
                {
                    UnlockProduct(defaultItemsIds[i]);
                }
            }
        }

        public static StoreProduct GetRandomLockedProduct()
        {
            return Database.GetRandomLockedProduct();
        }
    }
}