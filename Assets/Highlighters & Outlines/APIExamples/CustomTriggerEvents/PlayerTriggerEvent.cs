using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{
    /// <summary>
    /// Custom trigger event that is based on HighlighterTrigger logic.
    /// </summary>
    public class PlayerTriggerEvent : MonoBehaviour
    {
        // HighlighterTrigger component associated with this trigger event
        public HighlighterTrigger highlighterTrigger;

        // Layer mask for the trigger volume
        [SerializeField] private LayerMask volumeLayerMask;

        private void OnTriggerEnter(Collider other)
        {
            // Check if the collider entering the trigger volume is on a layer specified in the volumeLayerMask
            if (volumeLayerMask == (volumeLayerMask | (1 << other.gameObject.layer)))
            {
                // If the collider is on a valid layer, set the triggering state to true on the HighlighterTrigger component
                highlighterTrigger.ChangeTriggeringState(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Check if the collider exiting the trigger volume is on a layer specified in the volumeLayerMask
            if (volumeLayerMask == (volumeLayerMask | (1 << other.gameObject.layer)))
            {
                // If the collider is on a valid layer, set the triggering state to false on the HighlighterTrigger component
                highlighterTrigger.ChangeTriggeringState(false);
            }
        }
    }
}