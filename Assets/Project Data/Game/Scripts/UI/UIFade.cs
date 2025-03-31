using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class UIFade
    {
        [SerializeField] CanvasGroup fadeCanvasGroup;

        public void Show(float duration = 0.4f, bool immediately = false)
        {
            if (immediately)
            {
                fadeCanvasGroup.alpha = 1f;
                return;
            }

            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.DOFade(1f, duration, true);
        }

        public void Hide(float duration = 0.4f, bool immediately = false)
        {
            if (immediately)
            {
                fadeCanvasGroup.alpha = 0f;
                return;
            }

            fadeCanvasGroup.alpha = 1f;
            fadeCanvasGroup.DOFade(0, duration, true);
        }
    }
}
