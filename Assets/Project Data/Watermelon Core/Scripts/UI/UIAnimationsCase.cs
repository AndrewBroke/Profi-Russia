using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;

namespace Watermelon
{
    public class UIAnimationsCase
    {
        private RectTransform rectTransform;
        private Coroutine coroutine;
        private float startPositionY;

        public UIAnimationsCase(RectTransform rectTransform, Coroutine coroutine)
        {
            this.rectTransform = rectTransform;

            this.coroutine = coroutine;

            this.startPositionY = rectTransform.anchoredPosition.y;  
        }      
        

        public void Kill()
        {
            rectTransform.anchoredPosition = rectTransform.anchoredPosition.SetY(startPositionY);

            if (coroutine != null) UIAnimations.StopCaseCoroutine(coroutine);
        }
    }

   // public class 
}
