using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{

    /// <summary>
    /// Class derived from SimplePulsating that changes outline appearance when the HighlighterTrigger is currenly triggered.
    /// </summary>
    public class SimplePulsatingOutline : SimplePulsating
    {
        [Range(0, 0.1f)] public float maxThickness;
        [Range(0, 0.1f)] public float defaultThickness;

        [Range(0, 3)] public float maxPower;
        [Range(0, 3)] public float defaultPower;

        protected override void SetHighlighterSettings(float pong)
        {
            var thickness = Mathf.Lerp(defaultThickness, maxThickness, pong);
            var power = Mathf.Lerp(defaultPower, maxPower, pong);

            highlighter.Settings.MeshOutlineThickness = thickness;
            highlighter.Settings.InnerGlowFront.Power = power;
        }
    }
}