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

    private List<AudioSource> _effectSoundList;

    public void Init()
    {
        if(_audioSource == null)
        {
            _audioSource = new GameObject { name = "BGM" }.AddComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.playOnAwake = false;
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
        if(_audioClipDict.ContainsKey(clip.name) == false)
        {
            _audioClipDict.Add(clip.name, clip);
        }

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
                _effectSoundList.Add(sound.AudioSource);
                break;
        }
    }

    public void PlaySound(string path, SoundType type, bool isLoop = false, float pitch = 1.0f)
    {
        AudioClip clip;
        if (_audioClipDict.ContainsKey(path))
        {
            clip = _audioClipDict[path];
        }
        else
        {
            clip = Managers.Resource.Load<AudioClip>("Sound/" + path);
            _audioClipDict.Add(clip.name, clip);
        }

        PlaySound(clip, type, isLoop, pitch);
    }

    public void RemoveEffectSoundSource(AudioSource source)
    {
        _effectSoundList.Remove(source);
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
                foreach(AudioSource source in _effectSoundList)
                {
                    source.Stop();
                    Managers.Resource.Destroy(source.gameObject);
                }
                _effectSoundList.Clear();
                break;
        }
    }

    public void StopAllSound()
    {
        _audioSource.Stop();

        foreach (AudioSource source in _effectSoundList)
        {
            source.Stop();
            Managers.Resource.Destroy(source.gameObject);
        }
        _effectSoundList.Clear();
    }
}
