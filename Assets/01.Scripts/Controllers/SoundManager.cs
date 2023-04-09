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

    // 풀링되고 있는 모든 effect 오디오소스를 갇고 있는 리스트

    public void Init()
    {
        if(_audioSource == null)
        {
            _audioSource = new GameObject { name = "BGM" }.AddComponent<AudioSource>();
            //audioSource.outputAudioMixerGroup // 연결해주어야함

            Object.DontDestroyOnLoad(_audioSource);
        }

        // Effect Audio Source 앤 풀링
        // BGM Audio Source 동적 생성
    }

    /// <summary>
    /// 사운드 플레이 함수
    /// </summary>
    /// <param name="clip">오디오 클립</param>
    /// <param name="isLoop">반복 여부</param>
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
    /// 사운드 정지 함수
    /// </summary>
    public void StopSound(SoundType type)
    {
        switch (type)
        {
            case SoundType.Bgm:
                _audioSource.Stop();
                break;
            case SoundType.Effect:
                // 위에서 말한 리스트 풀링 후 클리어
                break;
        }
    }
}
