using UnityEngine;

namespace Watermelon
{
    [DefaultExecutionOrder(-10)]
    public class GameController : MonoBehaviour
    {
        private static GameController instance;

        [SerializeField] LevelDatabase levelDatabase;
        [SerializeField] LevelController levelController;
        [SerializeField] GameObject finishConfetti;

        private UIGame gameUI;

        public static event SimpleCallback OnCoinsChanged;
        public static LevelDatabase LevelDatabase => instance.levelDatabase;

        private static GameObject FinishConfetti => instance.finishConfetti;

        public static int CurrentLevelNumber
        {
            get => PrefsSettings.GetInt(PrefsSettings.Key.CurrentLevelNumber);
            set => PrefsSettings.SetInt(PrefsSettings.Key.CurrentLevelNumber, value);
        }

        public static int Coins
        {
            get => PrefsSettings.GetInt(PrefsSettings.Key.Coins);
            set
            {
                PrefsSettings.SetInt(PrefsSettings.Key.Coins, value);
                OnCoinsChanged?.Invoke();
            }
        }

        public void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            WallsBehaviour.Init();
            gameUI = UIController.GetPage<UIGame>();
            OnCoinsChanged?.Invoke();
        }

        public static void OnPlayButtonPressed()
        {
            UIController.HidePage(typeof(UIMainMenu));
            Tween.DelayedCall(UIMainMenu.HideDuration, () =>
            {
                UIController.ShowPage(typeof(UIGame));
            });

            LoadLevel();
        }

        private static void LoadLevel()
        {
            ResetLevel();

            Level level = LevelDatabase.GetLevel(CurrentLevelNumber);

            LevelController.LoadLevel(level);
            instance.gameUI.UpdateLevelText(CurrentLevelNumber);
            instance.gameUI.HidePartButtonsImmediately();

            AdsManager.ShowBanner();
        }

        public static void ResetLevel()
        {
            LevelController.PoolsHandler.ReturnEverythingToPool();
            LevelController.Rover.ResetRover();

            CameraController.ResetLevel();
            
            instance.gameUI.HidePartButtons();
            instance.gameUI.HidePlayButton();
        }

        public static void OnLevelCompleted()
        {
            AudioController.PlaySound(AudioController.Sounds.winSound);
            FinishConfetti.transform.position = LevelController.Finish.transform.position;
            FinishConfetti.SetActive(true);

            CurrentLevelNumber++;

            instance.gameUI.HidePartButtons();

            Tween.DelayedCall(0.5f, () =>
            {
                UIController.HidePage(typeof(UIGame));
                UIController.ShowPage(typeof(UIComplete));
            });
        }

        public static void OnLevelCompleteClosed()
        {
            LoadLevel();

            AdsManager.RequestInterstitial();
            AdsManager.ShowInterstitial(null);

            UIController.HidePage(typeof(UIComplete));
            Tween.DelayedCall(UIComplete.HideDuration, () =>
            {
                UIController.ShowPage(typeof(UIGame));
            });
        }

        public static void SkipLevel()
        {
            CurrentLevelNumber++;
            LoadLevel();
        }

        public static void ReplayCurrentLevel()
        {
            LoadLevel();
        }

        public static void ReturnToMainMenu()
        {
            ResetLevel();

            UIController.HidePage(typeof(UIGame));
            UIController.ShowPage(typeof(UIMainMenu));
        }
    }
}