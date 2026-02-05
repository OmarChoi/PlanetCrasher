using System;
using System.Collections;
using Lean.Pool;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public static event Action OnVolumeChanged;

    [SerializeField] private AudioSource _sfxAudioSourcePrefab;
    [SerializeField] private AudioSource _bgmAudioSource;

    private Sound _bgmSound;
    private Sound _sfxSound;
    private ISoundRepository _repository;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _repository = new LocalSoundRepository(AccountManager.Instance.Email);
    }

    private void Start()
    {
        Load();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public float GetBgmVolume() => _bgmSound.Volume;
    public float GetSfxVolume() => _sfxSound.Volume;

    public void SetBgmVolume(float volume)
    {
        _bgmSound.SetVolume(volume);
        _bgmAudioSource.volume = _bgmSound.Volume;
        Save();
        OnVolumeChanged?.Invoke();
    }

    public void SetSfxVolume(float volume)
    {
        _sfxSound.SetVolume(volume);
        Save();
        OnVolumeChanged?.Invoke();
    }

    public void PlayBgm(AudioClip clip)
    {
        if (clip == null) throw new ArgumentException("[SoundManager.cs] Try to Play Null BGM Clip");
        _bgmAudioSource.clip = clip;
        _bgmAudioSource.loop = true;
        _bgmAudioSource.volume = _bgmSound.Volume;
        _bgmAudioSource.Play();
    }

    public void StopBgm()
    {
        _bgmAudioSource.Stop();
    }

    public void PlaySfx(AudioClip clip, float pitch = 1f)
    {
        if (clip == null)
        {
            throw new ArgumentException("[SoundManager.cs] Try to Play Null SFX Clip");
        }
        AudioSource audioSource = LeanPool.Spawn(_sfxAudioSourcePrefab, transform);
        audioSource.clip = clip;
        audioSource.volume = _sfxSound.Volume;
        audioSource.pitch = pitch;
        audioSource.Play();
        StartCoroutine(PlaySfx_Coroutine(audioSource, clip.length / pitch));
    }

    private IEnumerator PlaySfx_Coroutine(AudioSource audioSource, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        LeanPool.Despawn(audioSource);
    }

    private void Save()
    {
        var data = new SoundSaveData
        {
            BgmVolume = _bgmSound.Volume,
            SfxVolume = _sfxSound.Volume
        };
        _repository.Save(data);
    }

    private void Load()
    {
        SoundSaveData data = _repository.Load();
        _bgmSound = new Sound(data.BgmVolume);
        _sfxSound = new Sound(data.SfxVolume);
    }
}