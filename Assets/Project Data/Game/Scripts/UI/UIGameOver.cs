using System;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class UIGameOver : UIPage
    {
        [Header("Settings")]
        [SerializeField] float noThanksDelay;

        [SerializeField] UIScalableObject levelFailed;

        [SerializeField] UIFade backgroundFade;

        [SerializeField] UIScalableObject continueButton;

        [Header("No Thanks Label")]
        [SerializeField] Button noThanksButton;
        [SerializeField] Text noThanksText;

        private TweenCase continuePingPongCase;

        [NonSerialized]
        public float HiddenPageDelay = 0f;

        public override void Init()
        {
            base.Init();
        }

        #region Show/Hide
        
        public override void Show()
        {
            if (isPageDisplayed)
                return;

            isPageDisplayed = true;
            canvas.enabled = true;

            levelFailed.Hide(immediately: true);
            continueButton.Hide(immediately: true);
            HideNoThanksButton();

            float fadeDuration = 0.3f;
            backgroundFade.Show(fadeDuration, false);

            Tween.DelayedCall(fadeDuration * 0.8f, delegate { 
            
                levelFailed.Show(false, scaleMultiplier: 1.1f);
                
                ShowNoThanksButton(noThanksDelay, immediately: false);

                continueButton.Show(false, scaleMultiplier: 1.05f);

                Tween.DelayedCall(continueButton.ShowHideDuration, delegate { 
                    
                    continuePingPongCase = continueButton.RectTransform.DOPingPongScale(1.0f, 1.05f, 0.9f, Ease.Type.QuadIn, Ease.Type.QuadOut, unscaledTime: true);
                    
                });                          
            });

        }

        public override void ShowImmediately()
        {
            if (isPageDisplayed)
                return;

            isPageDisplayed = true;
            canvas.enabled = true;

            backgroundFade.Show(immediately: true);
            levelFailed.Show(immediately: true);

            ShowNoThanksButton(noThanksDelay * 0.5f, immediately: false);

            continuePingPongCase = continueButton.RectTransform.DOPingPongScale(1.0f, 1.05f, 0.9f, Ease.Type.QuadIn, Ease.Type.QuadOut, unscaledTime: true);
            continueButton.Show(true);
        }

        public override void Hide()
        {
            if (!isPageDisplayed)
                return;

            HiddenPageDelay = 0.3f;

            backgroundFade.Hide(0.3f, false);
            continueButton.Hide(immediately: false);
            levelFailed.Hide(immediately: false);

            Tween.DelayedCall(0.3f, delegate {

                canvas.enabled = false;
                isPageDisplayed = false;

                if (continuePingPongCase != null && continuePingPongCase.isActive) continuePingPongCase.Kill();
            });
        }

        public override void HideImmediately()
        {
            if (!isPageDisplayed)
                return;

            canvas.enabled = false;
            isPageDisplayed = false;
            
            if (continuePingPongCase != null && continuePingPongCase.isActive) continuePingPongCase.Kill();
        }

        #endregion

        #region NoThanks Block

        public void ShowNoThanksButton(float delayToShow = 0.3f, bool immediately = true)
        {
            if (immediately)
            {
                noThanksButton.gameObject.SetActive(true);
                noThanksText.gameObject.SetActive(true);

                return;
            }

            Tween.DelayedCall(delayToShow, delegate { 

                noThanksButton.gameObject.SetActive(true);
                noThanksText.gameObject.SetActive(true);

            });
        }

        public void HideNoThanksButton()
        {
            noThanksButton.gameObject.SetActive(false);
            noThanksText.gameObject.SetActive(false);
        }

        #endregion

        #region Buttons 

        public void ContinueButton()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
            AdsManager.RequestRewardBasedVideo();
            AdsManager.ShowRewardBasedVideo((hasReward) =>
            {
                if (hasReward)
                {
                    GameController.SkipLevel();
                    UIController.HidePage(typeof(UIGameOver));
                    UIController.ShowPage(typeof(UIGame));
                }
                else
                {
                    NoThanksButton();
                }
            });
            
        }

        public void NoThanksButton()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
            GameController.ReplayCurrentLevel();
            UIController.HidePage(typeof(UIGameOver));
            UIController.ShowPage(typeof(UIGame));
        }

        #endregion
    }
}