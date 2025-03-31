using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class UIMainMenu : UIPage
    {
        private static readonly float HIDDEN_PAGE_DELAY = 0.55F;
        public readonly float STORE_AD_RIGHT_OFFSET_X = 300F;

        [Space]
        [SerializeField] RectTransform playButtonRect;
        [SerializeField] Button playButton;

        [Header("Coins Label")]
        [SerializeField] UIScalableObject coinsLabel;
        [SerializeField] Text coinsAmountsText;

        [SerializeField] UIMainMenuButton storeButtonRect;
        [SerializeField] UIMainMenuButton noAdsButtonRect;

        public static float HideDuration => HIDDEN_PAGE_DELAY;

        private TweenCase tapToPlayPingPong;
        private TweenCase showHideStoreAdButtonDelayTweenCase;

        private void OnEnable()
        {
            IAPManager.OnPurchaseComplete += OnAdPurchased;
            GameController.OnCoinsChanged += UpdateCashLabel;
        }

        private void OnDisable()
        {
            IAPManager.OnPurchaseComplete -= OnAdPurchased;
            GameController.OnCoinsChanged -= UpdateCashLabel;
        }

        public override void Init() // Called in the Start method
        {
            base.Init();

            UpdateCashLabel();

            storeButtonRect.Init(STORE_AD_RIGHT_OFFSET_X);
            noAdsButtonRect.Init(STORE_AD_RIGHT_OFFSET_X);
        }

        #region Show/Hide

        public override void Show()
        {
            if (isPageDisplayed)
                return;

            // KILL, RESET ANIMATED OBJECT

            showHideStoreAdButtonDelayTweenCase?.Kill();

            HideAdButton(true);

            //

            isPageDisplayed = true;
            canvas.enabled = true;

            ShowTapToPlay(false);

            coinsLabel.Show(false);
            storeButtonRect.Show(false);
            UILevelNumberText.Show(false);

            showHideStoreAdButtonDelayTweenCase = Tween.DelayedCall(0.12f, delegate
            {
                ShowAdButton(immediately: false);
            });

            SettingsPanel.ShowPanel(false);
        }

        public override void ShowImmediately()
        {
            if (isPageDisplayed)
                return;

            isPageDisplayed = true;
            canvas.enabled = true;

            ShowTapToPlay(true);

            coinsLabel.Show(true);
            storeButtonRect.Show(true);

            ShowAdButton(immediately: true);

            SettingsPanel.ShowPanel(true);
            UILevelNumberText.Show(true);
        }

        public override void Hide()
        {
            if (!isPageDisplayed)
                return;

            // KILL, RESET

            showHideStoreAdButtonDelayTweenCase?.Kill();

            //

            isPageDisplayed = false;

            HideTapToPlayText(false);

            coinsLabel.Hide(false);

            HideAdButton(immediately: false);

            showHideStoreAdButtonDelayTweenCase = Tween.DelayedCall(0.1f, delegate
            {
                storeButtonRect.Hide(immediately: false);
            });

            SettingsPanel.HidePanel(false);

            Tween.DelayedCall(HIDDEN_PAGE_DELAY, delegate
            {
                canvas.enabled = false;
            });
        }

        public override void HideImmediately()
        {
            if (!isPageDisplayed)
                return;

            canvas.enabled = false;
            isPageDisplayed = false;

            SettingsPanel.HidePanel(true);
        }

        #endregion

        #region Tap To Play Label

        public void ShowTapToPlay(bool immediately = true)
        {
            if (tapToPlayPingPong != null && tapToPlayPingPong.isActive)
                tapToPlayPingPong.Kill();

            playButton.interactable = true;

            if (immediately)
            {
                playButtonRect.localScale = Vector3.one;

                tapToPlayPingPong = playButtonRect.transform.DOPingPongScale(1.0f, 1.05f, 0.9f, Ease.Type.QuadIn, Ease.Type.QuadOut, unscaledTime: true);

                return;
            }

            // RESET
            playButtonRect.localScale = Vector3.zero;

            playButtonRect.DOPushScale(Vector3.one * 1.2f, Vector3.one, 0.35f, 0.2f, Ease.Type.CubicOut, Ease.Type.CubicIn).OnComplete(delegate
            {

                tapToPlayPingPong = playButtonRect.transform.DOPingPongScale(1.0f, 1.05f, 0.9f, Ease.Type.QuadIn, Ease.Type.QuadOut, unscaledTime: true);

            });

        }

        public void HideTapToPlayText(bool immediately = true)
        {
            if (tapToPlayPingPong != null && tapToPlayPingPong.isActive)
                tapToPlayPingPong.Kill();

            if (immediately)
            {
                playButtonRect.localScale = Vector3.zero;

                return;
            }

            playButtonRect.DOPushScale(Vector3.one * 1.2f, Vector3.zero, 0.2f, 0.35f, Ease.Type.CubicOut, Ease.Type.CubicIn);
        }

        #endregion

        #region Coins Label      

        public void UpdateCashLabel()
        {
            coinsAmountsText.text = GameController.Coins.ToString();
        }

        #endregion

        #region Ad Button Label

        private void ShowAdButton(bool immediately = false)
        {
            if (AdsManager.IsForcedAdEnabled())
            {
                noAdsButtonRect.Show(immediately);
            }
            else
            {
                noAdsButtonRect.Hide(immediately: true);
            }
        }

        private void HideAdButton(bool immediately = false)
        {
            noAdsButtonRect.Hide(immediately);
        }

        private void OnAdPurchased(ProductKeyType productKeyType)
        {
            if (productKeyType == ProductKeyType.NoAds)
            {
                HideAdButton(immediately: true);
            }
        }

        #endregion

        
        #region Buttons

        public void TapToPlayButton()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
            GameController.OnPlayButtonPressed();
            UIController.HidePage(typeof(UIMainMenu));
            Tween.DelayedCall(0.55f, () =>
            {
                UIController.ShowPage(typeof(UIGame));
            });
        }

        public void StoreButton()
        {
            UIController.HidePage(typeof(UIMainMenu));

            UIMainMenu uiMainMenu = UIController.GetPage<UIMainMenu>();
            UILevelNumberText.Hide(false);
            playButton.interactable = false;

            Tween.DelayedCall(HIDDEN_PAGE_DELAY, delegate
            {
                UIController.ShowPage(typeof(UIStore));
                playButton.interactable = true;
            });

            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }


        public void NoAdButton()
        {
            IAPManager.BuyProduct(ProductKeyType.NoAds);
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }

        #endregion
    }


}
