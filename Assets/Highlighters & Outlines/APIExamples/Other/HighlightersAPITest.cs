using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Highlighters
{
    /// <summary>
    /// A simple example of using the Highlighter class API.
    /// </summary>
    public class HighlightersAPITest : MonoBehaviour
    {
        // The Highlighter script component attached to this game object.
        private Highlighter highlighter;

        // Properties used to change the front overlay alpha value using an animation curve.
        public AnimationCurve animationCurve;
        public float duration;

        /// <summary>
        /// Gets the Highlighter component attached to this game object, if it exists.
        /// </summary>
        private void GetHighlighter()
        {
            if (!highlighter)
            {
                highlighter = GetComponent<Highlighter>();
            }
        }

        /// <summary>
        /// Toggles the enabled state of the Highlighter component.
        /// </summary>
        public void TestEnablingDisabling()
        {
            GetHighlighter();
            highlighter.enabled = !highlighter.enabled;
        }

        /// <summary>
        /// Adds a Highlighter component to this game object and configures its settings.
        /// </summary>
        public void TestAddingScripts()
        {
            highlighter = gameObject.AddComponent<Highlighter>();

            highlighter.GetRenderersInChildren();

            highlighter.Settings.DepthMask = DepthMask.FrontOnly;

            highlighter.Settings.UseOuterGlow = true;
            highlighter.Settings.BlurIterations = 50;
            highlighter.Settings.BoxBlurSize = 0.0243f;

            highlighter.Settings.UseOverlay = true;
            highlighter.Settings.OverlayFront.Color = new Color(0.6f, 0.6f, 0, 0);

            // Since we are adding new highlighter and enabling new features (UseOuterGlow and UseOverlay)
            // we need to validate highlighter
            highlighter.HighlighterValidate();
        }

        /// <summary>
        /// Tests changing various Highlighter settings and applying an impulse effect to the front overlay alpha value.
        /// </summary>
        public void TestSettings()
        {
            GetHighlighter();

            // Use the HighlighterUtilities static class to easily create a color impulse effect.
            StartCoroutine(HighlighterUtilities.ImpulseCurve(animationCurve, duration, updateTest));
        }

        /// <summary>
        /// A function used as an updater for the Highlighter settings.
        /// The value parameter is a value returned by the coroutine from the HighlighterUtilities class.
        /// </summary>
        /// <param name="value">The value to use for updating the Highlighter settings.</param>
        private void updateTest(float value)
        {
            var color = highlighter.Settings.OverlayFront.Color;
            color.a = value;
            highlighter.Settings.OverlayFront.Color = color;
        }
    }
}