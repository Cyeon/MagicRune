using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{
    public class ObjectOscilate : MonoBehaviour
    {
        public float minX;
        public float maxX;
        public float speed = 1.0f;

        void Update()
        {
            // Calculate oscillation value using ping-pong function
            float oscillation = Mathf.PingPong(Time.time * speed, 1.0f);

            // Calculate X position for the object
            float posX = Mathf.Lerp(minX, maxX, oscillation);

            // Set position of game object based on oscillation value
            transform.localPosition = new Vector3(posX, transform.localPosition.y, transform.localPosition.z);
        }

    }
}