using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{
    /// <summary>
    /// Custom trigger event that is based on HighlighterTrigger logic.
    /// </summary>
    [RequireComponent(typeof(HighlighterTrigger))]
    public class MousePointTriggerEvent : MonoBehaviour
    {
        // List of colliders that are associated with this trigger event
        private List<Collider> myColliders;
        // HighlighterTrigger component associated with this trigger event
        private HighlighterTrigger highlighterTrigger;

        // Camera used to cast rays for the mouse point trigger
        public Camera myCamera;

#if ENABLE_LEGACY_INPUT_MANAGER
        void Start()
        {
            // Get the HighlighterTrigger component associated with this trigger event
            highlighterTrigger = GetComponent<HighlighterTrigger>();
            // Initialize the list of colliders
            myColliders = new List<Collider>();
        }

        void Update()
        {
            // Flag indicating whether the trigger is being activated
            bool triggering = false;
            // Raycast to check if it hits any of the colliders associated with this trigger event
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);

            // Debug line to visualize the ray in the Scene view
            // Debug.DrawRay(ray.origin, ray.direction, Color.red);

            // If the list of colliders is empty, get all colliders in the children of this GameObject
            if (myColliders.Count == 0)
            {
                var arr = GetComponentsInChildren<Collider>();
                myColliders.AddRange(arr);
            }

            // Check if the raycast hits any of the colliders
            RaycastHit hit;
            foreach (var myCollider in myColliders)
            {
                if (myCollider.Raycast(ray, out hit, 1000))
                {
                    triggering = true;
                }
            }

            // Update the triggering state on the HighlighterTrigger component
            highlighterTrigger.ChangeTriggeringState(triggering);

            // If the trigger is being activated, check if the left mouse button is pressed and trigger the hit
            if (triggering)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    highlighterTrigger.TriggerHit();
                }
            }
        }
#endif
    }
}