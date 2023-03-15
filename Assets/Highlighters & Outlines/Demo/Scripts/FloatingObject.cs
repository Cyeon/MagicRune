using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Highlighters
{
    public class FloatingObject : MonoBehaviour
    {
        // The amplitude of the coin's oscillation
        public float amplitude = 0.5f;

        // The frequency of the coin's oscillation
        public float frequency = 1.0f;

        // The starting position of the coin
        private Vector3 startPos;

        void Start()
        {
            // Store the starting position of the coin
            startPos = transform.position;
        }

        void Update()
        {
            // Calculate the coin's new position based on a sinusoidal oscillation
            float newYPos = amplitude * Mathf.Sin(Time.time * frequency);
            transform.position = startPos + new Vector3(0, newYPos, 0);
        }
    }
}