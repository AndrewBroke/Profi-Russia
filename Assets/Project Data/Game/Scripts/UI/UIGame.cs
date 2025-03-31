using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class UIGame : UIPage
    {
        private readonly string LEVEL_LABEL = "LEVEL ";
        [SerializeField] Text levelText;
        [SerializeField] UIScalableObject coinsLabel;
        [SerializeField] UIScalableObject homeScalable;
        [SerializeField] Text coinsAmountText;

        [NonSerialized]
        public float HiddenPageDelay = 0f;

        private static UIGame instance;

        [SerializeField] RectTransform canvasRect;

        [SerializeField] List<RoverPartButton> partButtons;

        [SerializeField] RectTransform buttonsParent;

        [SerializeField] GameObject playButton;

        [Space]
        [SerializeField] GameObject replayButton;

        public static Vector2 CanvasSize => instance.canvasRect.sizeDelta;


        private void Awake()
        {
            instance = this;
        }

        private void OnEnable()
        {
            GameController.OnCoinsChanged += UpdateCashLabel;
        }

        private void OnDisable()
        {
            GameController.OnCoinsChanged -= UpdateCashLabel;
        }

        public override void Init()
        {
            base.Init();

            replayButton.gameObject.SetActive(false);
            HidePlayButton();
        }


        public void HidePartButtons()
        {
            buttonsParent.DOAnchoredPosition(Vector2.down * buttonsParent.sizeDelta.y, 0.5f).SetEasing(Ease.Type.SineOut);
            replayButton.gameObject.SetActive(false);
        }


        public void HidePartButtonsImmediately()
        {
            buttonsParent.DOAnchoredPosition(Vector2.down * buttonsParent.sizeDelta.y, 0).SetEasing(Ease.Type.SineOut);
            replayButton.gameObject.SetActive(false);
        }


        public void ShowPlayButton()
        {
            playButton.gameObject.SetActive(true);
            replayButton.gameObject.SetActive(false);
        }


        public void HidePlayButton()
        {
            playButton.gameObject.SetActive(false);
        }


        public void InitPartButtons(List<RoverPartBehavior> parts)
        {
            playButton.gameObject.SetActive(false);
            replayButton.gameObject.SetActive(true);

            buttonsParent.DOAnchoredPosition(Vector3.zero, 0.5f).SetEasing(Ease.Type.SineOut);

            var partsWithButtons = parts.FindAll((part) =>
            {
                if (part != null && part.Data != null)
                {
                    return part.Data.HasButton;
                }
                return false;
            });

            var orderedParts = new Dictionary<RoverPart, List<RoverPartBehavior>>();

            for (int i = 0; i < partsWithButtons.Count; i++)
            {
                var part = partsWithButtons[i];
                var data = part.Data;

                if (orderedParts.ContainsKey(data))
                {
                    orderedParts[data].Add(part);
                }
                else
                {
                    var list = new List<RoverPartBehavior>();
                    list.Add(part);

                    orderedParts.Add(data, list);
                }
            }

            int counter = 0;

            foreach (var data in orderedParts.Keys)
            {
                var button = partButtons[counter];

                button.gameObject.SetActive(true);
                button.Init(data, orderedParts[data]);

                counter++;
            }

            for (int i = counter; i < partButtons.Count; i++)
            {
                partButtons[i].gameObject.SetActive(false);
            }
        }

        #region Show/Hide

        public override void Hide()
        {
            if (!isPageDisplayed)
                return;

            coinsLabel.Hide(false, scaleMultiplier: 1.05f);
            homeScalable.Hide(false, scaleMultiplier: 1.05f);
            UILevelNumberText.Hide(false);

            HiddenPageDelay = coinsLabel.ShowHideDuration;

            Tween.DelayedCall(HiddenPageDelay, delegate
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

        public override void Show()
        {
            if (isPageDisplayed)
                return;

            isPageDisplayed = true;
            canvas.enabled = true;

            coinsLabel.Show(false, scaleMultiplier: 1.05f);
            homeScalable.Show(false, scaleMultiplier: 1.05f);
            UILevelNumberText.Show(false);
        }

        public override void ShowImmediately()
        {
            if (isPageDisplayed)
                return;

            isPageDisplayed = true;
            canvas.enabled = true;
        }

        #endregion

        #region Cash Label

        public void UpdateCashLabel()
        {
            coinsAmountText.text = GameController.Coins.ToString();
        }

        #endregion


        public void UpdateLevelText(int levelNumber)
        {
            levelText.text = LEVEL_LABEL + levelNumber;
        }

        #region Buttons

        public void PlayButton()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
            LevelController.LaunchVehicle();
        }

        public void RestartLevel()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
            GameController.ReplayCurrentLevel();
        }


        public void MainMenuButton()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
            GameController.ReturnToMainMenu();
        }

        #endregion

    }
}
