using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{
    /// <summary>
    /// Base class for enabling and disabling highlight based on TriggerWrapper events.
    /// </summary>
    public class SimpleActivation : TriggerWrapper
    {
        private void Start()
        {
            // Disable the highlighter by default
            DisableHighlighter();
        }

        // Method called when the trigger starts
        protected override void TriggeringStarted()
        {
            // Enable the highlighter when the trigger starts
            EnableHighlighter();
        }

        // Method called when the trigger ends
        protected override void TriggeringEnded()
        {
            // Disable the highlighter when the trigger ends
            DisableHighlighter();
        }

        // Method to enable the highlighter
        private void EnableHighlighter()
        {
            highlighter.enabled = true;
        }

        // Method to disable the highlighter
        private void DisableHighlighter()
        {
            highlighter.enabled = false;
        }
    }
}