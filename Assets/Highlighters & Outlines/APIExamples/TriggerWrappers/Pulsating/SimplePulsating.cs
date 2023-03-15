using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{

    /// <summary>
    /// Base class for adding pulsating effects based on TriggerWrapper events.
    /// </summary>
    public abstract class SimplePulsating : TriggerWrapper
    {
        public float frequency;
        public float threshold = 0.02f;
        public bool showHighlightAlways = false;

        private float currentPongValue;

        private float timeElapsed = 0;
        private bool canPulsate = true;

        private Coroutine returnToDefaultThickness;

        // How to use OnEnable and OnDisable functions in class inherited from TriggerWrapper
        protected override void OnEnable()
        {
            // We need to call this:
            base.OnEnable();

            // Here you can do whatever you want
            //Debug.Log("I am doing whatever I want.");
        }

        protected virtual void Start()
        {
            // We are making sure that default thickness is properly setup on start
            timeElapsed = -(Mathf.PI * 0.5f) / frequency;

            SetHighlighterSettings(0);

            if (showHighlightAlways) highlighter.enabled = true;
            else highlighter.enabled = false;
        }

        private void Update()
        {
            if (!canPulsate) return;
            if(highlighterTrigger.IsCurrentlyTriggered)
            {
                currentPongValue = CalculatePong();
                SetHighlighterSettings(currentPongValue);

            }
        }

        private float CalculatePong()
        {
            float pong = (Mathf.Sin(timeElapsed * frequency) + 1)/2;
            timeElapsed += Time.deltaTime;
            return pong;
        }

        /// <summary>
        /// Set highlighter settings values based on the pong value.
        /// </summary>
        /// <param name="pong"></param>
        protected abstract void SetHighlighterSettings(float pong);

        protected override void TriggeringEnded()
        {
            canPulsate = false;
            if (showHighlightAlways)
            {
                if (returnToDefaultThickness != null) StopCoroutine(returnToDefaultThickness);
                returnToDefaultThickness = StartCoroutine(ReturnToDefaultThickness());
            }
            else
            {
                highlighter.enabled = false;
            }
        }

        protected override void TriggeringStarted()
        {
            if (!showHighlightAlways)
            {
                highlighter.enabled = true;
                canPulsate = true;
            }
        }

        IEnumerator ReturnToDefaultThickness()
        {
            while (currentPongValue > 0 + threshold)
            {
                currentPongValue = CalculatePong();
                SetHighlighterSettings(currentPongValue);
                yield return null;
            }

            canPulsate = true;
        }
    }
}
