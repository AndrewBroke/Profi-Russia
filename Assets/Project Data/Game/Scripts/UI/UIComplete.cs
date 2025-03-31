using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Watermelon
{
    public class UIComplete : UIPage
    {
        [SerializeField] UIFade backgroundFade;

        [Space]
        [SerializeField] UIScalableObject levelCompleteLabel;

        [Space]
        [SerializeField] UIScalableObject rewardLabel;
        [SerializeField] Text rewardAmountText;

        [Header("Coins Label")]
        [SerializeField] UIScalableObject coinsLabel;
        [SerializeField] Text coinsAmountsText;

        [Space]
        [SerializeField] UIFade multiplyRewardButton;
        [SerializeField] UIFade noThanksButtonText;
        [SerializeField] Button noThanksButton;
        [SerializeField] UIFade continueButtonFade;
        [SerializeField] Button continueButton;

        public static float HideDuration => 0.25f;
        int coinsHash = CurrencyCloudController.StringToHash("Coins");

        private TweenCase noThanksAppearTween;

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

            // RESET

            rewardLabel.Hide(immediately: true);
            multiplyRewardButton.Hide(immediately: true);
            noThanksButtonText.Hide(immediately: true);
            noThanksButton.interactable = false;
            coinsLabel.Hide(immediately: true);
            continueButtonFade.Hide(immediately: true);
            continueButton.gameObject.SetActive(false);

            backgroundFade.Show(duration: 0.3f);
            levelCompleteLabel.Show();

            Tween.DelayedCall(levelCompleteLabel.ShowHideDuration, delegate
            {
                coinsAmountsText.text = GameController.Coins.ToString();
                coinsLabel.Show();
                ShowRewardLabel(LevelController.LevelReward, false, 0.3f, delegate
                {
                    rewardLabel.RectTransform.DOPushScale(Vector3.one * 1.1f, Vector3.one, 0.2f, 0.2f).OnComplete(delegate
                    {
                        CurrencyCloudController.SpawnCurrency(coinsHash, rewardLabel.RectTransform, coinsLabel.RectTransform, 10, "", () =>
                        {
                            GameController.Coins += LevelController.LevelReward;
                            coinsAmountsText.text = GameController.Coins.ToString();
                            multiplyRewardButton.Show();

                            noThanksAppearTween = Tween.DelayedCall(1.5f, delegate
                             {
                                 noThanksButtonText.Show();
                                 noThanksButton.interactable = true;
                             });
                        });
                    });
                });
            });
        }

        public override void ShowImmediately()
        {
            if (isPageDisplayed)
                return;

            isPageDisplayed = true;
            canvas.enabled = true;
            coinsLabel.Show(true);

            backgroundFade.Show(immediately: true);

            ShowRewardLabel(LevelController.LevelReward, true);

            multiplyRewardButton.Show(immediately: true);
            levelCompleteLabel.Show(true);
        }

        public override void Hide()
        {
            if (!isPageDisplayed)
                return;

            backgroundFade.Hide(HideDuration, false);
            coinsLabel.Hide(false);

            Tween.DelayedCall(HideDuration, delegate
            {
                canvas.enabled = false;
                isPageDisplayed = false;
            });
        }

        public override void HideImmediately()
        {
            if (!isPageDisplayed)
                return;

            canvas.enabled = false;
            isPageDisplayed = false;
        }


        #endregion

        #region RewardLabel

        public void ShowRewardLabel(float rewardAmounts, bool immediately = false, float duration = 0.3f, Action onComplted = null)
        {
            rewardLabel.Show(immediately);

            if (immediately)
            {
                rewardAmountText.text = "+" + rewardAmounts;
                onComplted?.Invoke();

                return;
            }

            rewardAmountText.text = "+" + 0;

            Tween.DoFloat(0, rewardAmounts, duration, (float value) =>
            {

                rewardAmountText.text = "+" + (int)value;
            }).OnComplete(delegate
            {

                onComplted?.Invoke();
            });
        }

        #endregion

        #region Buttons

        public void MultiplyRewardButton()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);

            if(noThanksAppearTween != null && noThanksAppearTween.isActive)
            {
                noThanksAppearTween.Kill();
            }

            AdsManager.ShowRewardBasedVideo((bool success) =>
            {
                if (success)
                {
                    int rewardMult = 3;

                    noThanksButton.interactable = false;
                    noThanksButtonText.Hide(immediately: true);
                    multiplyRewardButton.Hide(immediately: true);

                    ShowRewardLabel(LevelController.LevelReward * rewardMult, false, 0.3f, delegate
                     {
                         CurrencyCloudController.SpawnCurrency(coinsHash, rewardLabel.RectTransform, coinsLabel.RectTransform, 10, "", () =>
                         {
                             GameController.Coins += LevelController.LevelReward * rewardMult;
                             coinsAmountsText.text = GameController.Coins.ToString();

                             noThanksButton.interactable = true;
                             continueButton.gameObject.SetActive(true);
                             continueButtonFade.Show();
                         });
                     });
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
            GameController.OnLevelCompleteClosed();
        }

        public void HomeButton()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }

        #endregion
    }
}
