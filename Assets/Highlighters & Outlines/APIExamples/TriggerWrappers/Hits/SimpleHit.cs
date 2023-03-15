using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters {

    /// <summary>
    /// Base class for adding hits functionality to your object based on TriggerWrapper class.
    /// </summary>
    public class SimpleHit : TriggerWrapper
    {
        // Properties 
        public AnimationCurve overlayAlpha;
        public float duration;

        // Our coroutine that will be responsible for hit impulse
        private Coroutine hit;

        protected virtual void Start()
        {
            DisableHighlighter();
        }

        protected override void HitTrigger()
        {
            
            if (hit != null) StopCoroutine(hit);

            // Using HighlighterUtilities class to make our life easier


            // You could always replace EnableHighlighter with null and call it here
            hit = StartCoroutine(HighlighterUtilities.ImpulseCurve(overlayAlpha, duration, UpdateOverlaySettings, EnableHighlighter, DisableHighlighter)); 

            // And call it from here like that:
            // EnableHighlighter();

            // This does the same but C# actions help to keep the code clean
        }

        private void UpdateOverlaySettings(float value)
        {
            var col = highlighter.Settings.OverlayFront.Color;
            col.a = value;
            highlighter.Settings.OverlayFront.Color = col;

            var col1 = highlighter.Settings.InnerGlowFront.Color;
            col1.a = value;
            highlighter.Settings.InnerGlowFront.Color = col1;
        }

        private void EnableHighlighter()
        {
            highlighter.enabled = true;
        }

        private void DisableHighlighter()
        {
            highlighter.enabled = false;
        }
    }
}