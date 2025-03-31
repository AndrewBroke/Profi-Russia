using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class ProductPreview : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float hideDuration = 0.5f;

        [Space]
        [SerializeField] StoreViewMode viewMode;

        [Space]
        [SerializeField] Transform previewHolderTransform;

        [ShowIf("Is3DProductPreview")]
        [SerializeField] GameObject preview3DModel;        
        [ShowIf("Is3DProductPreview")]
        [SerializeField] Camera previewCameraRenderer;

        [ShowIf("Is2DProductPreview")]
        [SerializeField] SpriteRenderer preview2D;

        [ShowIf("Is3DProductPreview")]
        [Header("Model Settings")]
        [SerializeField] ProductSetting productSettings; 

        private void OnEnable()
        {
            StoreController.OnProductSelected += productSettings.ApplyProduct;
        }

        private void OnDisable()
        {
            StoreController.OnProductSelected -= productSettings.ApplyProduct;
        }

        public void Init()
        {
            ApplyMode();
        }

        public void HidePreviewMode(bool immediately = false)
        {
            if (immediately)
            {
                previewCameraRenderer.enabled = false;

                if (viewMode == StoreViewMode.View3D)
                {
                    preview3DModel.gameObject.SetActive(false);
                }
                else
                {
                    preview2D.gameObject.SetActive(false);
                }
            }
            else
            {
                previewHolderTransform.transform.DOPushScale(Vector3.one * 1.05f, Vector3.zero, hideDuration * 0.36f, hideDuration * 0.64f, Ease.Type.SineOut, Ease.Type.SineIn).OnComplete(delegate
                {
                    previewCameraRenderer.enabled = false;


                    if (viewMode == StoreViewMode.View3D)
                    {
                        preview3DModel.gameObject.SetActive(false);
                    }
                    else
                    {
                        preview2D.gameObject.SetActive(false);
                    }
                });
            }          
        }

        private void ApplyMode()
        {
            previewHolderTransform.gameObject.SetActive(true);
            previewHolderTransform.transform.localScale = Vector3.one;
            previewCameraRenderer.enabled = true;

            if (viewMode == StoreViewMode.View3D)
            {
                // Turn off 2D Preview
                if (preview2D != null && preview2D.gameObject.activeSelf) preview2D.gameObject.SetActive(false);

                // Turn on 3D Preview
                if (preview3DModel != null && !preview3DModel.gameObject.activeSelf) preview3DModel.gameObject.SetActive(true);

                // Aplly Preview
                //productSettings.ApplyProduct(StoreController.GetSelectedProduct(StoreProductType.CharacterColorProduct));
                //productSettings.ApplyProduct(StoreController.GetSelectedProduct(StoreProductType.CharacterHatProduct));
            }
            else
            {
                // Turn off 3D Preview
                if (preview3DModel != null && preview3DModel.gameObject.activeSelf) preview3DModel.gameObject.SetActive(false);

                // Turn on 2D Preview
                if (preview2D != null && !preview2D.gameObject.activeSelf) preview2D.gameObject.SetActive(true);
               
                // Aplly Preview
                //previewImage.sprite = ;
            }
        }   
    
        #region Editor Conditionals
        private bool Is3DProductPreview()
        {
            return viewMode == StoreViewMode.View3D;
        }

        private bool Is2DProductPreview()
        {
            return viewMode == StoreViewMode.View2D;
        }
        #endregion
    }

    public enum StoreViewMode
    {
        View2D = 0,
        View3D = 1,
    }
}
