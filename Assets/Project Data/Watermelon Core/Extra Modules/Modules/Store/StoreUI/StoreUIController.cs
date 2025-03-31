using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Watermelon
{
    //Store Module v0.9.2
    [HelpURL("https://docs.google.com/document/d/1SS9_U59ACe1kSrd2nxSmbqvWtTu6Uv1SQzcyocQLZB8/edit")]
    public class StoreUIController : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private static StoreUIController instance;
        public static readonly string STORE_ITEM_POOL_NAME = "StoreItem";
        public static readonly string PAGES_POOL_NAME = "StorePage";
        public static readonly string PAGE_DOT_POOL_NAME = "PageDot";

        [Header("Settings")]
        [SerializeField] float swipeThereshold = 0.2f;
        private static float SwipeThereshold => instance.swipeThereshold;

        [Header("References")]
        [SerializeField] GameObject storeItemPrefab;
        private static GameObject StoreItemPrefab => instance.storeItemPrefab;
        [SerializeField] GameObject pageDotPrefab;
        private static GameObject PageDotPrefab => instance.pageDotPrefab;
        [SerializeField] GameObject pagePrefab;
        private static GameObject PagePrefab => instance.pagePrefab;


        [Space(5f)]
        [SerializeField] Transform storeObjectsHolder;
        private static Transform StoreObjectsHolder => instance.storeObjectsHolder;
        [SerializeField] Transform pagesHolderTransform;
        private static Transform PagesHolderTransform => instance.pagesHolderTransform;
        [SerializeField] Transform pageDotsHolderTransform;
        private static Transform PageDotsHolderTransform => instance.pageDotsHolderTransform;
        [SerializeField] Transform previewHolderTransform;
        private static Transform PreviewHolderTransform => instance.previewHolderTransform;


        [Space(5f)]
        [SerializeField] RectTransform storeCanvasRect;
        private static RectTransform StoreCanvasRect => instance.storeCanvasRect;
        [SerializeField] GameObject unlockRandomButtonObject;
        private static GameObject UnlockRandomButtonObject => instance.unlockRandomButtonObject;
        [SerializeField] GameObject getCoinsButtonObject;
        private static GameObject GetCoinsButtonObject => instance.getCoinsButtonObject;
        [SerializeField] Text unlockRandomPriceText;
        private static Text UnlockRandomPriceText => instance.unlockRandomPriceText;
        [SerializeField] Text coinsForAdsText;
        private static Text CoinsForAdsText => instance.coinsForAdsText;
        [SerializeField] Transform coinsPanelTransform;
        private static Transform CoinsPanelTransform => instance.coinsPanelTransform;
        [SerializeField] Text coinsAmountText;
        private static Text CoinsAmountText => instance.coinsAmountText;

        [Space(5f)]
        [SerializeField] List<StoreTab> tabsList = new List<StoreTab>();
        private static List<StoreTab> TabsList => instance.tabsList;

        private static Dictionary<StorePageName, List<StoreProduct>> productsByGroupDictionary;
        private static List<Image> pageDotsImagesList = new List<Image>();
        private static List<StorePage> pagesList = new List<StorePage>();
        private static List<StoreProduct> skinsOnCurrentPageList = new List<StoreProduct>();
        private static List<int> closedSkinsOnCurrentPageIndexes = new List<int>();
        private static List<int> pagesAmountForEachProductType = new List<int>();

        private static HorizontalLayoutGroup pageDotsLayout;
        private static Pool storeItemPool;
        private static Pool pagesPool;
        private static Pool pageDotsPool;
        private static CanvasScaler canvasScalerRef;

        private static Vector3 pagesHolderLocalPosition;
        public static StoreProductType CurrentProductType { get; private set; }

        private static bool isAnimationRunning;
        private static float canvasWidth;
        private static int currentPageIndex;
        private static int pagesAmount;

        private void Awake()
        {
            instance = this;
            canvasScalerRef = storeCanvasRect.GetComponent<CanvasScaler>();

            ClearPreviewObjects();
            SetupUIForScreenRatio();

            canvasWidth = canvasScalerRef.referenceResolution.x;
            pageDotsLayout = pageDotsHolderTransform.GetComponent<HorizontalLayoutGroup>();
            storeItemPool = PoolManager.AddPool(new PoolSettings(STORE_ITEM_POOL_NAME, storeItemPrefab, 10, true));
            pagesPool = PoolManager.AddPool(new PoolSettings(PAGES_POOL_NAME, pagePrefab, 3, true, pagesHolderTransform));
            pageDotsPool = PoolManager.AddPool(new PoolSettings(PAGE_DOT_POOL_NAME, pageDotPrefab, 3, true, pageDotsHolderTransform));
        }

        private void ClearPreviewObjects()
        {
            if (pagesHolderTransform.childCount > 0)
            {
                for (int i = 0; i < pagesHolderTransform.childCount; i++)
                {
                    if (pagesHolderTransform.GetChild(i).name.Contains("[Page Preview]"))
                    {
                        Destroy(pagesHolderTransform.GetChild(i).gameObject);
                    }
                }
            }

            ClearOldPreviewModels();
        }

        private void Start()
        {
            pagesAmountForEachProductType = StoreController.Database.GetPagesAmountPerProducts();

            StoreController.OnProductSelected += OnNewSkinSelected;
        }

        private void SetupUIForScreenRatio()
        {
            float screenRatio = Screen.width / (float)Screen.height;

            if (screenRatio > canvasScalerRef.referenceResolution.x / canvasScalerRef.referenceResolution.y)
            {
                canvasScalerRef.matchWidthOrHeight = 1f;
            }
        }

        public static void OpenStore()
        {
            Debug.Log("[Store Module UI] Add coins text initialization here.");
            CoinsAmountText.text = 250.ToString();
            StoreObjectsHolder.gameObject.SetActive(true);
            InitStoreUI(StoreProductType.EnvironmentSkin);
        }

        private static void InitStoreUI(StoreProductType type)
        {
            CurrentProductType = type;
            storeItemPool.ReturnToPoolEverything(true);
            pagesPool.ReturnToPoolEverything();
            pageDotsPool.ReturnToPoolEverything();
            pageDotsImagesList.Clear();
            isAnimationRunning = false;

            pagesAmount = pagesAmountForEachProductType[(int)type];
            UnlockRandomPriceText.text = StoreController.Database.GetProductPrice(CurrentProductType).ToString();
            CoinsForAdsText.text = "GET " + StoreController.Database.CoinsForAdsAmount.ToString();

            productsByGroupDictionary = StoreController.Database.GetProductsByPageDictionary(CurrentProductType);
            currentPageIndex = (int)StoreController.GetProduct(StoreController.GetSelectedProductSkinID(CurrentProductType)).Page;
            pagesHolderLocalPosition = Vector3.zero.SetX(currentPageIndex * -Mathf.Clamp(Screen.width, canvasWidth, Screen.width));
            PagesHolderTransform.localPosition = pagesHolderLocalPosition;
            Debug.Log("screen width: " + Screen.width + " can width: " + canvasWidth + "   " + currentPageIndex);

            for (int i = 0; i < pagesAmount; i++)
            {
                Transform page = pagesPool.GetPooledObject().GetComponent<Transform>();
                page.localPosition = page.localPosition.SetX(Mathf.Clamp(Screen.width, canvasWidth, Screen.width) * i);
                pagesList.Add(page.GetComponent<StorePage>());

                if (pagesAmount > 1)
                {
                    pageDotsLayout.enabled = true;
                    pageDotsImagesList.Add(pageDotsPool.GetPooledObject().GetComponent<Image>());
                    pageDotsImagesList[i].color = Color.white.SetAlpha(currentPageIndex == i ? 1f : 0.4f);
                }

                InitPage(i);
            }

            UpdateProductPreview();
            UpdateCurrentPage(false);
            UpdateTabsState();

            Tween.DelayedCall(0.1f, () => pageDotsLayout.enabled = true);
        }

        private static void InitPage(int pageIndex)
        {
            pagesList[pageIndex].Init(productsByGroupDictionary[(StorePageName)pageIndex]);
        }

        private static void UpdateCurrentPage(bool redrawStorePage)
        {
            UpdatePagePoints();

            skinsOnCurrentPageList = productsByGroupDictionary[(StorePageName)currentPageIndex];

            closedSkinsOnCurrentPageIndexes.Clear();

            for (int i = 0; i < skinsOnCurrentPageList.Count; i++)
            {
                if (!skinsOnCurrentPageList[i].IsUnlocked() && skinsOnCurrentPageList[i].BehaviourType != BehaviourType.Dummy)
                {
                    closedSkinsOnCurrentPageIndexes.Add(i);
                }
            }

            UnlockRandomButtonObject.SetActive(closedSkinsOnCurrentPageIndexes.Count > 0);

            if (redrawStorePage)
            {
                pagesList[currentPageIndex].UpdatePage();
            }
        }

        private static void UpdatePagePoints()
        {
            for (int i = 0; i < pageDotsImagesList.Count; i++)
            {
                pageDotsImagesList[i].color = Color.white.SetAlpha(currentPageIndex == i ? 1f : 0.4f);
            }
        }

        private IEnumerator RandomUnlockAnimation()
        {
            isAnimationRunning = true;
            float delay = 0.05f;
            int itemToUnlockIndex = closedSkinsOnCurrentPageIndexes[Random.Range(0, closedSkinsOnCurrentPageIndexes.Count)];
            List<StoreItemUI> storeItemsList = pagesList[currentPageIndex].StoreItemsList;

            if (closedSkinsOnCurrentPageIndexes.Count > 1)
            {
                for (int i = 0; i < storeItemsList.Count; i++)
                {
                    storeItemsList[i].SetHighlightState(false);
                }

                Tween.DoFloat(0.05f, 0.5f, 3f, (float newValue) => delay = newValue);

                while (delay < 0.5f)
                {

                    closedSkinsOnCurrentPageIndexes.Remove(itemToUnlockIndex);
                    int newIndex = closedSkinsOnCurrentPageIndexes[Random.Range(0, closedSkinsOnCurrentPageIndexes.Count)];
                    closedSkinsOnCurrentPageIndexes.Add(itemToUnlockIndex);
                    itemToUnlockIndex = newIndex;


                    for (int i = 0; i < storeItemsList.Count; i++)
                    {
                        storeItemsList[i].SetHighlightState(i == itemToUnlockIndex);
                    }

                    yield return new WaitForSeconds(delay);
                }

                yield return new WaitForSeconds(delay * 0.5f);
            }

            if (StoreController.TryToBuyProduct(skinsOnCurrentPageList[itemToUnlockIndex]))
            {
                UpdateCurrentPage(true);
            }

            isAnimationRunning = false;
        }

        public static void OnNewSkinSelected(StoreProduct product)
        {
            for (int i = 0; i < pagesList.Count; i++)
            {
                pagesList[i].UpdatePage();
            }

            UpdateProductPreview();
        }

        public static void OnTabPressed(StoreProductType productType)
        {
            if (CurrentProductType != productType)
            {
                InitStoreUI(productType);
            }
        }

        private static void UpdateTabsState()
        {
            for (int i = 0; i < TabsList.Count; i++)
            {
                TabsList[i].SetActiveState(TabsList[i].Type == CurrentProductType);
            }
        }

        private static void UpdateProductPreview()
        {
            ClearOldPreviewModels();

            //Instantiate(StoreController.GetSelectedProduct(CurrentProductType).SkinPrefab, PreviewHolderTransform.position, Quaternion.Euler(0f, 140f, 0f), PreviewHolderTransform);
        }

        private static void ClearOldPreviewModels()
        {
            if (PreviewHolderTransform.childCount > 0)
            {
                for (int i = 0; i < PreviewHolderTransform.childCount; i++)
                {
                    Destroy(PreviewHolderTransform.GetChild(i).gameObject);
                }
            }
        }

        #region Swipe

        public void OnDrag(PointerEventData data)
        {
            float difference = data.pressPosition.x - data.position.x;

            pagesHolderTransform.localPosition = Vector3.Lerp(pagesHolderLocalPosition, pagesHolderLocalPosition - Vector3.zero.SetX(difference), 0.8f);
        }

        public void OnEndDrag(PointerEventData data)
        {
            float percentage = (data.pressPosition.x - data.position.x) / canvasWidth;
            float pageDeltaSign = Mathf.Sign(percentage);

            if (Mathf.Abs(percentage) >= swipeThereshold && ((pageDeltaSign < 0 && currentPageIndex > 0) || (pageDeltaSign > 0 && currentPageIndex < pagesAmount - 1)))
            {
                Vector3 newPosition = pagesHolderLocalPosition;

                currentPageIndex += (int)pageDeltaSign;
                UpdateCurrentPage(false);
                Debug.Log("screen width: " + Screen.width + " can width: " + canvasWidth + "   " + currentPageIndex);


                newPosition += Vector3.zero.SetX(-Mathf.Clamp(Screen.width, canvasWidth, Screen.width) * pageDeltaSign);
                pagesHolderTransform.DOLocalMove(newPosition, 0.1f).OnComplete(() => pagesHolderLocalPosition = newPosition);
            }
            else
            {
                pagesHolderTransform.DOLocalMove(pagesHolderLocalPosition, 0.1f);
            }
        }


        #endregion

        #region Buttons

        public void UnlockRandomButton()
        {
            if (closedSkinsOnCurrentPageIndexes.Count <= 0)
                return;

            Debug.Log("[Store Module UI] Add additional checks here.");
            if (!isAnimationRunning)
            {
                StartCoroutine(RandomUnlockAnimation());
            }
        }

        public void GetCoinsForAds()
        {
            AdsManager.ShowRewardBasedVideo(AdsManager.Settings.RewardedVideoType, (bool haveReward) =>
             {
                 if (haveReward)
                 {
                     Debug.Log("[Store Module UI] Add coins for ads functionality here.");
                 }
             });
        }

        public void CloseStoreButton()
        {
            storeObjectsHolder.gameObject.SetActive(false);
        }

        #endregion

        #region Developement

        // currently unused because of Unity Editor bug: Screen.width returns different value if Game view is not active (click on this button in the ispector deactivates Game view which leads to wrong initialization of the store)
        //[Button("Open Store")]
        public void OpenStoreButtonDev()
        {
            OpenStore();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                OpenStoreButtonDev();
            }
        }

        #endregion
    }
}