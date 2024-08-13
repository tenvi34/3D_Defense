using UnityEngine;

public class BackgroundMusicManager : DD_Singleton<BackgroundMusicManager>
{
    private AudioSource _audioSource;

    protected override void Awake()
    {
        base.Awake();
        InitializeAudioSource();
    }

    private void InitializeAudioSource()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        _audioSource.playOnAwake = false;
        _audioSource.loop = true;
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (_audioSource == null)
        {
            InitializeAudioSource();
        }

        // AudioSource가 비활성화되어 있다면 활성화합니다.
        if (!_audioSource.enabled)
        {
            _audioSource.enabled = true;
        }

        if (_audioSource.clip != musicClip)
        {
            _audioSource.clip = musicClip;
        }

        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (_audioSource != null && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        if (_audioSource != null)
        {
            _audioSource.volume = volume;
        }
    }
}