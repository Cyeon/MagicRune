using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioSource AudioSource => _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Init(AudioClip clip, float pitch = 1.0f)
    {
        if(_audioSource == null)
        {
            TryGetComponent<AudioSource>(out _audioSource);
        }

        _audioSource.clip = clip;
        _audioSource.pitch = pitch;
        _audioSource.Play();

        StartCoroutine(PoolCoroutine(clip.length));
    }

    private IEnumerator PoolCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Managers.Sound.RemoveEffectSoundSource(_audioSource);
        Managers.Resource.Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
