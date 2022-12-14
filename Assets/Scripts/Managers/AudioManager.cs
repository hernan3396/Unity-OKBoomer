using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public enum OST
    {
        MainMenu,
        Tutorial,
        RestoDeLevels,
    }

    public enum SFX
    {
        PointingFinger,
        PowerGloves,
        TronsEncom,
        PlayerHit,
        PlayerDeath,
        Dialogue,
        FinishDialogue,
        PickupHealth,
        PickupAmmo
    }

    private static AudioManager _instance;

    [SerializeField] private List<AudioScriptable> _audioList;
    [SerializeField] private List<AudioScriptable> _sfxList;

    #region Sources
    [Header("Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;
    #endregion

    #region Mixer
    [Header("Mixer")]
    [SerializeField] private AudioMixer _mixer;
    private bool _isFading = false;
    #endregion

    [SerializeField, Range(1, 10)] private int _fadeSpeed = 5;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(gameObject);

        EventManager.PlayMusic += FadeBetweenMusic;
        EventManager.PlaySound += PlaySound;
        EventManager.Play3dSound += CreateSoundAndPlay;
        EventManager.PlayOwn3dSound += RandomizeExternalSound;
    }

    public void PlaySound(SFX audioItem)
    {
        AudioScriptable audioScript = _sfxList[(int)audioItem];

        if (!audioScript) return;

        _soundSource.volume = Random.Range(audioScript.volume.x, audioScript.volume.y);
        _soundSource.pitch = Random.Range(audioScript.pitch.x, audioScript.pitch.y);

        _soundSource.PlayOneShot(audioScript.GetAudioClip(0));
    }

    public void PlayMusic(OST audioItem, bool randomSound = false, int index = 0)
    {
        if (_isFading) return;

        AudioScriptable audioScript = _audioList[(int)audioItem];
        if (!audioScript || !audioScript.IsMusic) return;

        if (randomSound)
            _musicSource.clip = audioScript.GetRandom();
        else
            _musicSource.clip = audioScript.GetAudioClip(index);

        _musicSource.volume = Random.Range(audioScript.volume.x, audioScript.volume.y);
        _musicSource.pitch = Random.Range(audioScript.pitch.x, audioScript.pitch.y);

        _musicSource.Play();
    }

    private void FadeBetweenMusic(OST musicClip)
    {
        _isFading = true;
        StartCoroutine("FadeOut", musicClip);
    }

    private IEnumerator FadeOut(OST musicClip)
    {
        float musicVolume;
        _mixer.GetFloat("MusicVolume", out musicVolume);

        while (musicVolume > -80)
        {
            _mixer.SetFloat("MusicVolume", musicVolume -= _fadeSpeed);
            yield return new WaitForSeconds(0.1f);
        }

        _isFading = false;
        PlayMusic(musicClip);
        StartCoroutine("FadeIn");
    }

    private IEnumerator FadeIn()
    {
        float musicVolume;
        _mixer.GetFloat("MusicVolume", out musicVolume);

        while (musicVolume < -20)
        {
            _mixer.SetFloat("MusicVolume", musicVolume += _fadeSpeed);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // si bien se podrian juntar con
    // PlaySound() creo que como trata de
    // un sonido 3d es valido separarlos 
    public void RandomizeExternalSound(AudioSource audioSource, AudioScriptable audioScript)
    {
        if (!audioScript) return;

        audioSource.clip = audioScript.GetAudioClip(0);
        audioSource.volume = Random.Range(audioScript.volume.x, audioScript.volume.y);
        audioSource.pitch = Random.Range(audioScript.pitch.x, audioScript.pitch.y);
        audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioScript.AnimCurve);

        audioSource.PlayOneShot(audioScript.GetAudioClip(0));
    }

    public void CreateSoundAndPlay(Vector3 pos, AudioScriptable audioScript)
    {
        GameObject go = new GameObject();
        go.transform.position = pos;

        AudioSource source = go.AddComponent<AudioSource>();
        source.spatialBlend = 1.0f;
        source.rolloffMode = AudioRolloffMode.Custom;
        source.maxDistance = 200;
        source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioScript.AnimCurve);
        source.volume = Random.Range(audioScript.volume.x, audioScript.volume.y);
        source.pitch = Random.Range(audioScript.pitch.x, audioScript.pitch.y);

        source.clip = audioScript.GetAudioClip(0);
        source.PlayOneShot(source.clip);

        Destroy(go, source.clip.length / source.pitch);
    }

    private void OnDestroy()
    {
        EventManager.PlayMusic -= FadeBetweenMusic;
        EventManager.PlaySound -= PlaySound;
        EventManager.Play3dSound -= CreateSoundAndPlay;
        EventManager.PlayOwn3dSound -= RandomizeExternalSound;
    }
}
