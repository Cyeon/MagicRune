using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{
    /// <summary>
    /// The TriggerWrapper script is a class that other classes should be derived from. It is used to automate the process of changing the properties of the highlighter from the code, based on the state of the HighlighterTrigger script.
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    [RequireComponent(typeof(HighlighterTrigger))]
    public abstract class TriggerWrapper : MonoBehaviour
    {
        /// <summary>
        /// Use it to check the status of highlighterTrigger.IsCurrentlyTriggered.
        /// </summary>
        protected HighlighterTrigger highlighterTrigger = null;

        /// <summary>
        /// Modify properties of this object.
        /// </summary>
        protected Highlighter highlighter = null;

        private void GetRequiredComponents()
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
        /// Subscribes to HighlighterTrigger events. When overriding in child class, call base.OnEnable()
        /// </summary>
        protected virtual void OnEnable()
        {
            GetRequiredComponents();
            highlighterTrigger.OnTriggeringStarted += TriggeringStarted;
            highlighterTrigger.OnTriggeringEnded += TriggeringEnded;
            highlighterTrigger.OnHitTrigger += HitTrigger;
        }


        /// <summary>
        /// Unsubscribes to HighlighterTrigger events. When overriding in child class, call base.OnDisable()
        /// </summary>
        protected virtual void OnDisable()
        {
            GetRequiredComponents();
            highlighterTrigger.OnTriggeringStarted -= TriggeringStarted;
            highlighterTrigger.OnTriggeringEnded -= TriggeringEnded;
            highlighterTrigger.OnHitTrigger -= HitTrigger;
        }

        /// <summary>
        /// Override the function and add your custom code.
        /// </summary>
        protected virtual void TriggeringStarted()
        {

        }

        /// <summary>
        /// Override the function and add your custom code.
        /// </summary>
        protected virtual void TriggeringEnded()
        {

        }

        /// <summary>
        /// Override the function and add your custom code.
        /// </summary>
        protected virtual void HitTrigger()
        {

        }
    }
}