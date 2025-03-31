using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;

namespace Watermelon
{
    public class UIAnimations : MonoBehaviour
    {
        private static UIAnimations instance;

        [Header("Higlight Animation 1")]
        [SerializeField] int rotationsAmount = 5;
        [SerializeField] float targetAngel = -10;
        [SerializeField] float rotationsDelay = 0.01f;
        [SerializeField] float jumpPositionDeltaY = 40f;
        [SerializeField] float jumpAnimationDuration = 0.2f;
        [SerializeField] float animationsDelay = 5f;

        [SerializeField] Ease.Type rotationEaseType;
        [SerializeField] Ease.Type jumpEaseType;

        [Header("Higlight Animation 2")]
        [SerializeField] float targetScale;
        [SerializeField] float scaleDuration;
        
        [SerializeField] float fullRotationDuration;
        [SerializeField] float scaleDuration_2;
        [SerializeField] Ease.Type scaleEasing;

        private static int RotationsAmount => instance.rotationsAmount;
        private static float TargetAngel => instance.targetAngel;
        private static float RotationsDelay => instance.rotationsDelay;
        private static float JumpPositionDeltaY => instance.jumpPositionDeltaY;
        private static float JumpAnimationDuration => instance.jumpAnimationDuration;
        private static float AnimationsDelay => instance.animationsDelay;
        private static Ease.Type RotationEaseType => instance.rotationEaseType;
        private static Ease.Type JumpEaseType => instance.jumpEaseType;

        //
        private static float TargetScale => instance.targetScale;
        private static float ScaleDuration => instance.scaleDuration;
        
        private static float FullRotationDuration => instance.fullRotationDuration;
        private static float ScaleDuration_2 => instance.scaleDuration_2;
        private static Ease.Type ScaleEasing => instance.scaleEasing;

        private static Coroutine rotationZCoroutine;
        private static Coroutine higlightAnimation1Coroutine;
        private static Coroutine scaleAnimtionCoroutine;

        public Coroutine HiglightAnimation1CoroutineRef => higlightAnimation1Coroutine;


        private static TweenCase rotationZTween;
        private static TweenCase jumpYTween;
        private static TweenCase scaleAnimation;

        private void Awake()
        {
            instance = this;
        }

        public static void StopCaseCoroutine(Coroutine coroutine)
        {
            instance.StopCoroutine(coroutine);
        }

        #region Higlight Animation 1

        public static UIAnimationsCase HiglightAnimation1(RectTransform rectTransform)
        {
            return new UIAnimationsCase(rectTransform, instance.StartCoroutine(HiglightAnimation1Coroutine(rectTransform)));
        }

        private static IEnumerator HiglightAnimation1Coroutine(RectTransform rectTransform)
        {
            if (rotationZCoroutine != null)
                instance.StopCoroutine(rotationZCoroutine);

            rotationZCoroutine = instance.StartCoroutine(RotationZAnimation(rectTransform, RotationsAmount, TargetAngel, RotationsDelay, RotationEaseType));

            yield return new WaitForSeconds((RotationsDelay * RotationsAmount) + 0.2f);

            JumpY(rectTransform, JumpPositionDeltaY, JumpEaseType);

            //yield return new WaitForSeconds(AnimationsDelay);
        }

        private static IEnumerator RotationZAnimation(Transform transform, int rotationsAmount, float angel, float rotationsDelay, Ease.Type easeType)
        {
            int iteration = 0;

            while (iteration <= rotationsAmount)
            {
                if (rotationZTween != null && rotationZTween.isActive)
                    rotationZTween.Kill();

                Tween.DoFloat(transform.rotation.z, angel, rotationsDelay, (float value) =>
                {
                    transform.rotation = Quaternion.Euler(0, 0, value);
                });//.SetEasing(easeType);
                iteration++;
                angel *= -1;

                if (iteration == rotationsAmount)
                    angel = 0f;


                yield return new WaitForSeconds(rotationsDelay);
            }
        }

        private static void JumpY(RectTransform rectTransform, float targetPositionY, Ease.Type easeType)
        {
            if (jumpYTween != null && jumpYTween.isActive)
                jumpYTween.Kill();

            var startPositionY = rectTransform.anchoredPosition.y;

            targetPositionY = rectTransform.anchoredPosition.y + targetPositionY;


            jumpYTween = rectTransform.DOAnchoredPosition(new Vector3(rectTransform.anchoredPosition.x, targetPositionY, 0f), JumpAnimationDuration)
               .SetEasing(easeType).OnComplete(() =>
               {
                   rectTransform.DOAnchoredPosition(new Vector3(rectTransform.anchoredPosition.x, startPositionY, 0f), JumpAnimationDuration).SetEasing(easeType);
               });
        }

        #endregion

        #region Higlight Animation 2

        public static UIAnimationsCase HighlightAnimation2(RectTransform rectTransform)
        {
            return new UIAnimationsCase(rectTransform, instance.StartCoroutine(HiglightAnimation2Coroutine(rectTransform)));
        }

        private  static IEnumerator HiglightAnimation2Coroutine(RectTransform rectTransform)
        {
            float currentTime = ScaleDuration_2;

            if (currentTime > 0) scaleAnimtionCoroutine = instance.StartCoroutine(ScaleAnimation(rectTransform));

            while(currentTime >= 0)
            {
                currentTime -= Time.deltaTime;

                yield return null;

                if (currentTime <= 0)
                {
                    instance.StopCoroutine(scaleAnimtionCoroutine);
                    yield return new WaitForSeconds(ScaleDuration * 2f);
                    FullRotate(rectTransform);
                }
            }            
        }

        
        private static void FullRotate(RectTransform rectTransform)
        {
            Tween.DoFloat(rectTransform.transform.rotation.z, -360f, FullRotationDuration, (float value) => {
                rectTransform.transform.rotation = Quaternion.Euler(0f, 0f, value);
            });
        }

        public static IEnumerator ScaleAnimation(RectTransform rectTransform)
        {
            while (true)
            {
                scaleAnimation = rectTransform.DOScale(new Vector3(TargetScale, TargetScale, TargetScale), ScaleDuration).SetEasing(ScaleEasing).OnComplete(() =>
                {
                    rectTransform.DOScale(new Vector3(1f, 1f, 1f), ScaleDuration);
                });
                yield return new WaitForSeconds(ScaleDuration * 2f);
            }
        }

        #endregion

        #region Test Buttons
        [LineSpacer("Test Buttons")]

        [SerializeField] RectTransform testTransform;
        private UIAnimationsCase uiCaseTest;

        [Button("Higlight Animation 11")]
        public void TestAnimation1()
        {
            uiCaseTest = HiglightAnimation1(testTransform);
            // Debug.Log(ui.rectTransform);
        }

        [Button("Kill Higlight Animation 11")]
        public void TestKillAnimation1()
        {
            uiCaseTest?.Kill();            
        }

        [Button("Highlight Animation 2")]
        public void TestAnimation2()
        {
            uiCaseTest = HighlightAnimation2(testTransform);
        }

        #endregion
    }
}
