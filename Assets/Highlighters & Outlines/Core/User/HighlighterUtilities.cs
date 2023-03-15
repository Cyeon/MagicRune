using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Static class that contains useful coroutines that you can use to create various effects with the highlighter.
/// </summary>
public static class HighlighterUtilities
{
    public static IEnumerator ImpulseSmoothstep(float from, float to, float duration, Action<float> updateValues, Action onStart = null, Action onEnd = null)
    {
        // Variable holding elapsed time 
        float elapsedTime = 0f;

        // Here you can add stuff that should happen when the coroutine is called
        if (onStart != null) onStart();

        while (elapsedTime < duration)
        {
            // Calculates value of the curve in time
            elapsedTime += Time.deltaTime;
            float value = Mathf.SmoothStep(from, to, elapsedTime / duration);

            // Add here the variables you want to change based on the curve value
            // ------------------------------------------

            if (updateValues != null) updateValues(value);

            // ------------------------------------------

            // Wait for the next frame
            yield return null;

            // Or use other functions like WaitForSeconds
            // See: https://docs.unity3d.com/Manual/Coroutines.html 
        }

        // Here you can add stuff that should happen when the coroutine has ended
        //highlighter.Settings.UseOverlay = false;
        if (onEnd != null) onEnd();
    }

    public static IEnumerator ImpulseLinear(float from, float to, float duration, Action<float> updateValues, Action onStart = null, Action onEnd = null)
    {
        // Variable holding elapsed time 
        float elapsedTime = 0f;

        // Here you can add stuff that should happen when the coroutine is called
        if (onStart != null) onStart();

        while (elapsedTime < duration)
        {
            // Calculates value of the curve in time
            elapsedTime += Time.deltaTime;
            float value = Mathf.Lerp(from, to, elapsedTime / duration);

            // Add here the variables you want to change based on the curve value
            // ------------------------------------------

            if (updateValues != null) updateValues(value);

            // ------------------------------------------

            // Wait for the next frame
            yield return null;

            // Or use other functions like WaitForSeconds
            // See: https://docs.unity3d.com/Manual/Coroutines.html 
        }

        // Here you can add stuff that should happen when the coroutine has ended
        //highlighter.Settings.UseOverlay = false;
        if (onEnd != null) onEnd();
    }

    public static IEnumerator ImpulseCurve(AnimationCurve curve, float duration, Action<float> updateValues, Action onStart = null, Action onEnd = null)
    {
        // Variable holding elapsed time 
        float elapsedTime = 0f;

        // Here you can add stuff that should happen when the coroutine is called
        if(onStart != null) onStart();

        while (elapsedTime < duration)
        {
            // Calculates value of the curve in time
            elapsedTime += Time.deltaTime;
            float value = curve.Evaluate(elapsedTime / duration);

            // Add here the variables you want to change based on the curve value
            // ------------------------------------------

            if (updateValues != null) updateValues(value);

            // ------------------------------------------

            // Wait for the next frame
            yield return null;

            // Or use other functions like WaitForSeconds
            // See: https://docs.unity3d.com/Manual/Coroutines.html 
        }

        // Here you can add stuff that should happen when the coroutine has ended
        //highlighter.Settings.UseOverlay = false;
        if (onEnd != null) onEnd();
    }

    public static IEnumerator ImpulseGradient(Gradient gradient, float duration, Action<Color> updateValues, Action onStart = null, Action onEnd = null)
    {
        // Variable holding elapsed time 
        float elapsedTime = 0f;

        // Here you can add stuff that should happen when the coroutine is called
        if (onStart != null) onStart();

        while (elapsedTime < duration)
        {
            // Calculates value of the curve in time
            elapsedTime += Time.deltaTime;
            Color value = gradient.Evaluate(elapsedTime / duration);

            // Add here the variables you want to change based on the curve value
            // ------------------------------------------

            if (updateValues != null) updateValues(value);

            // ------------------------------------------

            // Wait for the next frame
            yield return null;

            // Or use other functions like WaitForSeconds
            // See: https://docs.unity3d.com/Manual/Coroutines.html 
        }

        // Here you can add stuff that should happen when the coroutine has ended
        if (onEnd != null) onEnd();
    }
}
