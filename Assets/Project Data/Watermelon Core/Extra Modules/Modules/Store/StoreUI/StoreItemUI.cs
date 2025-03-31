using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class StoreItemUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] Color outlineColorNeutral;
        [SerializeField] Color outlineColorSelected;       

        [Header("References")]
        [SerializeField] Image productImage;
        [SerializeField] Image outlineImage;
        [SerializeField] Image lockedImage;

        public StoreProduct ProductRef { get; private set; }

        public void Init(StoreProduct product, bool isSelected)
        {
            ProductRef = product;

            UpdateItem(isSelected);
        }

        public void UpdateItem(bool isSelected)
        {
            if (ProductRef == null)
            {
                Debug.LogError("Store Items is not initialized.");
                return;
            }


            outlineImage.color = isSelected ? outlineColorSelected : outlineColorNeutral;

            /*if (ProductRef.Type == StoreProductType.CharacterColorProduct)
            {
                productImage.color = ((CharacterColorProduct)ProductRef).SkinColor;
                productImage.sprite = null;
            }
            else
            {
                productImage.color = Color.white;
                productImage.sprite = ProductRef.UnlockedIcon;
            }*/

            productImage.sprite = ProductRef.UnlockedIcon;
            lockedImage.enabled = !ProductRef.IsUnlocked();
        }

        public void OnClick()
        {
            if (ProductRef.IsUnlocked())
            {
                StoreController.TryToSelectProduct(ProductRef.ID);
            }
        }

        public void SetHighlightState(bool active)
        {
            outlineImage.color = active ? outlineColorSelected : outlineColorNeutral;
        }
    }
}