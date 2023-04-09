using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    Bgm,
    Effect
}

public class SoundManager
{
    private AudioSource _audioSource = null;

    private Dictionary<string, AudioClip> _audioClipDict = new Dictionary<string, AudioClip>();

    // Ǯ���ǰ� �ִ� ��� effect ������ҽ��� ���� �ִ� ����Ʈ

    public void Init()
    {
        if(_audioSource == null)
        {
            _audioSource = new GameObject { name = "BGM" }.AddComponent<AudioSource>();
            //audioSource.outputAudioMixerGroup // �������־����

            Object.DontDestroyOnLoad(_audioSource);
        }

        // Effect Audio Source �� Ǯ��
        // BGM Audio Source ���� ����
    }

    /// <summary>
    /// ���� �÷��� �Լ�
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="isLoop">�ݺ� ����</param>
    public void PlaySound(AudioClip clip, SoundType type, bool isLoop = false, float pitch = 1.0f)
    {
        switch(type)
        {
            case SoundType.Bgm:
                _audioSource.Stop();
                _audioSource.clip = clip;
                _audioSource.pitch = pitch;
                _audioSource.loop = isLoop;
                _audioSource.Play();
                break;
            case SoundType.Effect:
                SoundPool sound = Managers.Resource.Instantiate("Sound/" + clip.name).GetComponent<SoundPool>();
                sound.Init(clip, pitch);
                break;
        }
    }

    public void PlaySound(string path, SoundType type, bool isLoop = false, float pitch = 1.0f)
    {
        AudioClip clip = Managers.Resource.Load<AudioClip>("Sound/" + path);

        PlaySound(clip, type, isLoop, pitch);
    }

    /// <summary>
    /// ���� ���� �Լ�
    /// </summary>
    public void StopSound(SoundType type)
    {
        switch (type)
        {
            case SoundType.Bgm:
                _audioSource.Stop();
                break;
            case SoundType.Effect:
                // ������ ���� ����Ʈ Ǯ�� �� Ŭ����
                break;
        }
    }
}
