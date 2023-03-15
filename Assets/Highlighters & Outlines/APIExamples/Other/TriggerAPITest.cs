using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{
    /// <summary>
    /// A simple example of using the HighlighterTrigger script without the TriggerWrapper helper class.
    /// </summary>
    [RequireComponent(typeof(HighlighterTrigger))]
    public class TriggerAPITest : MonoBehaviour
    {
        // Properties used to change some values of the highlighter settings when the trigger starts and ends.
        public AnimationCurve enterCurve;
        public Gradient enterGradient;
        public AnimationCurve exitCurve;
        public float duration;

        // The HighlighterTrigger and Highlighter scripts attached to this game object.
        private HighlighterTrigger highlighterTrigger;
        private Highlighter highlighter;

        /// <summary>
        /// Gets the HighlighterTrigger and Highlighter components attached to this game object, if they exist.
        /// </summary>
        private void GetHighlighterTrigger()
        {
            if (highlighterTrigger == null)
            {
                highlighterTrigger = GetComponent<HighlighterTrigger>();
            }

            if (highlighter == null)
            {
                highlighter = GetComponent<Highlighter>();
            }
        }

        /// <summary>
        /// Subscribes to the HighlighterTrigger events when this component is enabled.
        /// </summary>
        private void OnEnable()
        {
            GetHighlighterTrigger();
            highlighterTrigger.OnTriggeringStarted += TriggeringStarted;
            highlighterTrigger.OnTriggeringEnded += TriggeringEnded;
        }

        /// <summary>
        /// Unsubscribes from the HighlighterTrigger events when this component is disabled.
        /// </summary>
        private void OnDisable()
        {
            GetHighlighterTrigger();
            highlighterTrigger.OnTriggeringStarted -= TriggeringStarted;
            highlighterTrigger.OnTriggeringEnded -= TriggeringEnded;
        }

        private void Update()
        {
            // Check the current state of the HighlighterTrigger.
            if (highlighterTrigger.IsCurrentlyTriggered)
            {
                // Debug.Log("I am currently triggering.");
            }
        }

        /// <summary>
        /// Handles the start of the trigger.
        /// </summary>
        private void TriggeringStarted()
        {
            // Use the HighlighterUtilities class to apply an impulse effect to the blur intensity and outline color.
            StartCoroutine(HighlighterUtilities.ImpulseCurve(enterCurve, duration, BlurIntensity, EnableHighlighter));
            StartCoroutine(HighlighterUtilities.ImpulseGradient(enterGradient, duration, OutlineColor, EnableHighlighter));
        }

        /// <summary>
        /// Handles the end of the trigger.
        /// </summary>
        private void TriggeringEnded()
        {
            // Use the HighlighterUtilities class to apply an impulse effect to the blur intensity.
            StartCoroutine(HighlighterUtilities.ImpulseCurve(exitCurve, duration, BlurIntensity, null, DisableHighlighter));
        }

        // This method sets the blur intensity for the highlighter object
        // and adjusts the alpha value of the outer glow color to match the intensity.
        private void BlurIntensity(float value)
        {
            // Set the blur intensity on the highlighter's settings
            highlighter.Settings.BlurIntensity = value;

            // Get the current outer glow color and adjust the alpha value to match the intensity
            var color = highlighter.Settings.OuterGlowColorFront;
            color.a = value;
            highlighter.Settings.OuterGlowColorFront = color;
        }

        // This method sets the outer glow color for the highlighter object.
        private void OutlineColor(Color value)
        {
            highlighter.Settings.OuterGlowColorFront = value;
        }

        // This method enables the highlighter object.
        private void EnableHighlighter()
        {
            highlighter.enabled = true;
        }

        // This method disables the highlighter object.
        private void DisableHighlighter()
        {
            highlighter.enabled = false;
        }
    }
}