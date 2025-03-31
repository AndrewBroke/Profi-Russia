using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class ProductSetting
    {
        [SerializeField] SkinnedMeshRenderer skin;
        [SerializeField] Transform hatHolder;

        public Transform HatHolder => hatHolder;

        public void ApplyProduct(StoreProduct product)
        {
            /*if (product.Type == StoreProductType.CharacterColorProduct)
            {
                skin.material = ((CharacterColorProduct)product).SkinMaterial;
            }
            if (product.Type == StoreProductType.CharacterHatProduct)
            {
                OnNewHatSelected((CharacterHatProduct)product);
            }*/
        }

        /*public void OnNewHatSelected(CharacterHatProduct hatProduct)
        {
            ClearHatHolder();

            var hat = hatProduct.ProductPool.GetPooledObject();

            hat.transform.SetParent(hatHolder);

            hat.transform.localScale = Vector3.one;
            hat.transform.localPosition = hatProduct.ModelPosition;
            hat.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        private void ClearHatHolder()
        {
            if (hatHolder.childCount == 0) return;

            for (int i = 0; i < hatHolder.childCount; i++)
            {
                hatHolder.GetChild(i).gameObject.SetActive(false);
            }
        }*/
    }
}
