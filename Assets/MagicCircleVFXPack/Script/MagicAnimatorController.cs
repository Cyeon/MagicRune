using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicAnimatorController : MonoBehaviour
{
    public Animator _animator;
    public UnityEvent[] FrameEvent;

    public void CallFrameEvent(int number)
    {
        if (FrameEvent.Length > number)
        {
            FrameEvent[number].Invoke();
        }
    }

    [ContextMenu("Play")]
    public void Play()
    {
        _animator.SetTrigger("Show");
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        _animator.SetTrigger("Over");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Play();
        }
    }
}
