using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Highlighters
{
    /// <summary>
    /// The HighlighterTrigger script is a tool for highlighters that allows you to easily add basic trigger functionality or create custom trigger events.
    /// </summary>
    public class HighlighterTrigger : MonoBehaviour
    {
        public enum TriggerMode
        {
            ObjectEnterVolume, CameraRaycast, CustomEvents
        }

        [Tooltip("ObjectEnterVolume: detects the triggering state of the object collider." +
            "CameraRaycast: creates a ray from the camera that triggers the highlighter when the ray hits it." +
            "CustomEvents: use when you use a custom triggering script.")]
        public TriggerMode TriggeringMode = TriggerMode.CustomEvents;

        private Collider myCollider;

        [SerializeField] private LayerMask volumeLayerMask;
        [SerializeField] private Camera myCamera;
        [SerializeField] private float maxDistanceFromCamera = 0;
        [SerializeField] private bool drawDebugLine = false;

        [SerializeField] private bool isCurrentlyTriggeredDebug = false;
        private bool isCurrentlyTriggered = false;

        /// <summary>
        /// Whether the object is in a triggering state.
        /// </summary>
        public bool IsCurrentlyTriggered
        {
            get => isCurrentlyTriggered;
        }


        public delegate void TriggeringEnd();

        /// <summary>
        /// Event triggered when the state of the trigger changes to true.
        /// </summary>
        public event TriggeringEnd OnTriggeringEnded;


        public delegate void TriggeringStart();

        /// <summary>
        /// Event triggered when the state of the trigger changes to false.
        /// </summary>
        public event TriggeringStart OnTriggeringStarted;

        public delegate void Hit();

        /// <summary>
        /// Event for hit trigger.
        /// </summary>
        public event Hit OnHitTrigger;

        // Setting up the delegates for the trigger events
        // ===============================================

        private void TriggeringEnded()
        {
            if (OnTriggeringEnded != null)
            {
                OnTriggeringEnded();
            }
        }

        private void TriggeringStarted()
        {
            if (OnTriggeringStarted != null)
            {
                OnTriggeringStarted();
            }
        }

        private void HitTrigger()
        {
            if (OnHitTrigger != null)
            {
                OnHitTrigger();
            }
        }

        // ===============================================

        /// <summary>
        /// Call this function on each update to update the state of the trigger. 
        /// Updates the value of IsCurrentlyTriggered and calls the OnTriggeringEnded and OnTriggeringStarted events automatically when the state of the trigger changes.
        /// </summary>
        /// <param name="isTriggered"> Custom trigger event status. </param>
        public void ChangeTriggeringState(bool isTriggered)
        {
            updateTriggeringState(isTriggered);
        }

        /// <summary>
        /// All events subscribed to the OnHitTrigger event will be triggered.
        /// </summary>
        public void TriggerHit()
        {
            HitTrigger();
        }

        public void Update()
        {
            if (TriggeringMode == TriggerMode.CameraRaycast) cameraTrigger();

            if (isCurrentlyTriggeredDebug) isCurrentlyTriggered = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TriggeringMode != TriggerMode.ObjectEnterVolume) return;
            if(volumeLayerMask == (volumeLayerMask | (1 << other.gameObject.layer)))
            {
                updateTriggeringState(true);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (TriggeringMode != TriggerMode.ObjectEnterVolume) return;

            if (volumeLayerMask == (volumeLayerMask | (1 << other.gameObject.layer)))
            {
                updateTriggeringState(true);
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (TriggeringMode != TriggerMode.ObjectEnterVolume) return;

            if (volumeLayerMask == (volumeLayerMask | (1 << other.gameObject.layer)))
            {
                updateTriggeringState(false);
            }
        }

        private void cameraTrigger()
        {
            if (myCamera == null) return;
            var cameraTransform = myCamera.transform;

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (drawDebugLine)
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * maxDistanceFromCamera, Color.red);
            }

            bool currenlyTriggered = false;

            RaycastHit hit;

            if (myCollider == null) myCollider = GetComponent<Collider>();

            if (myCollider.Raycast(ray, out hit, maxDistanceFromCamera))
            {
                currenlyTriggered = true;

            }
            updateTriggeringState(currenlyTriggered);
        }

        private void updateTriggeringState(bool currenlyTriggered)
        {
            if (isCurrentlyTriggeredDebug) return;

            if (currenlyTriggered)
            {
                // If triggering state in the last frame was false
                if(!isCurrentlyTriggered)
                {
                    TriggeringStarted();
                }

                isCurrentlyTriggered = true;
            }
            else
            {
                // If triggering state in the last frame was true
                if (isCurrentlyTriggered)
                {
                    TriggeringEnded();
                }

                isCurrentlyTriggered = false;
            }
        }

        /// <summary>
        /// Wrapper for TriggeringStarted() function. Use for testing.
        /// </summary>
        public void TestTriggeringStarted()
        {
            TriggeringStarted();
        }

        /// <summary>
        /// Wrapper for TriggeringEnded() function. Use for testing.
        /// </summary>
        public void TestTriggeringEnded()
        {
            TriggeringEnded();
        }
    }
}