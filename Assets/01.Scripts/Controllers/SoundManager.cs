using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    Master,
    Bgm,
    Effect
}

public class SoundManager
{
    [SerializeField] private AudioSource _master;
    [SerializeField] private AudioSource _effect;
    [SerializeField] private AudioSource _bgm;

    private AudioSource _audioSource = null;

    public void Init()
    {
        _audioSource.outputAudioMixerGroup = Managers.Resource.Load<AudioMixerGroup>("adf");
    }

    /// <summary>
    /// ���� �÷��� �Լ�
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="isLoop">�ݺ� ����</param>
    public void PlaySound(AudioClip clip, SoundType type, bool isLoop = false)
    {
        switch(type)
        {
            case SoundType.Master:
                _audioSource = _master;
                break;

            case SoundType.Bgm:
                _audioSource = _bgm;
                break;

            case SoundType.Effect:
                _audioSource = _effect;
                break;
        }

        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.loop = isLoop;
        _audioSource.Play();
    }


    /// <summary>
    /// ���� ���� �Լ�
    /// </summary>
    public void StopSound(SoundType type)
    {
        switch (type)
        {
            case SoundType.Master:
                _audioSource = _master;
                break;

            case SoundType.Bgm:
                _audioSource = _bgm;
                break;

            case SoundType.Effect:
                _audioSource = _effect;
                break;
        }

        _audioSource.Stop();
    }
}
