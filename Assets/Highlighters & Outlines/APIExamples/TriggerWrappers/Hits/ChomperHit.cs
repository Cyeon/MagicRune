using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{
    /// <summary>
    /// A class derived from SimpleHit that also triggers the Hit animator trigger when an object is hit 
    /// </summary>
    public class ChomperHit : SimpleHit
    {
        private Animator animator;

        // Remember to call base.Start() and Base.HitTrigger()
        // It is just simple inheritance.

        // If you are lost in this, just copy code from SimpleHit and modify it based on your needs.

        protected override void Start()
        {
            base.Start();
            animator = GetComponent<Animator>();
        }

        protected override void HitTrigger()
        {
            base.HitTrigger();
            animator.SetTrigger("Hit");
        }
    }
}