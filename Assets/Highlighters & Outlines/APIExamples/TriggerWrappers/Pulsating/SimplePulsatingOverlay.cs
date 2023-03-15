using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{
    /// <summary>
    /// Class derived from SimplePulsating that changes overlay appearance when the HighlighterTrigger is currenly triggered.
    /// </summary>
    public class SimplePulsatingOverlay : SimplePulsating
    {
        [Range(0, 1)] public float maxAlpha;
        [Range(0, 1)] public float defaultAlpha;

        protected override void SetHighlighterSettings(float pong)
        {
            var alpha = Mathf.Lerp(defaultAlpha, maxAlpha, pong);

            var col = highlighter.Settings.OverlayFront.Color;
            col.a = alpha;
            highlighter.Settings.OverlayFront.Color = col;
        }
    }
}