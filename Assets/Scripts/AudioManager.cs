using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioSource musicSource, sfxSource, voiceOverSource;

    [System.Serializable]
    public class SoundEffect
    {
        public string sfxName;
        public AudioClip clip;
    }

    [System.Serializable]
    public class VoiceOverData
    {
        public string voName;
        public AudioClip clip;
    }

    [System.Serializable]
    public class SceneMusicMapping
    {
        public string musicName;
        public AudioClip musicClip;
        public List<string> sceneNames;
    }

    [SerializeField] List<SoundEffect> sfxList = new();
    [SerializeField] List<VoiceOverData> voiceOverList = new();
    [SerializeField] List<SceneMusicMapping> sceneMusicMappings = new();

    readonly Dictionary<string, AudioClip> sfxDict = new();
    readonly Dictionary<string, AudioClip> voDict = new();
    readonly Dictionary<string, AudioClip> sceneMusicDict = new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeDictionaries();
        SceneManager.activeSceneChanged += OnSceneChanged;
        PlayMusicInCurrentScene();
    }

    void OnDestroy() => SceneManager.activeSceneChanged -= OnSceneChanged;

    void InitializeDictionaries()
    {
        foreach (var s in sfxList)
            if (!string.IsNullOrEmpty(s.sfxName) && s.clip)
                sfxDict[s.sfxName] = s.clip;

        foreach (var v in voiceOverList)
            if (!string.IsNullOrEmpty(v.voName) && v.clip)
                voDict[v.voName] = v.clip;

        foreach (var m in sceneMusicMappings)
        {
            if (!m.musicClip) continue;

            foreach (var scene in m.sceneNames)
                if (!string.IsNullOrEmpty(scene))
                    sceneMusicDict[scene] = m.musicClip;
        }
    }

    void OnSceneChanged(Scene _, Scene __) => PlayMusicInCurrentScene();

    public void PlayMusicInCurrentScene()
    {
        if (!musicSource) return;

        string scene = SceneManager.GetActiveScene().name;

        if (sceneMusicDict.TryGetValue(scene, out var clip))
        {
            if (musicSource.clip == clip && musicSource.isPlaying) return;

            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
            musicSource.clip = null;
        }
    }

    public void PlaySFX(string name)
    {
        if (sfxSource && sfxDict.TryGetValue(name, out var clip))
            sfxSource.PlayOneShot(clip);
        else if (sfxSource)
            Debug.LogWarning($"[AudioManager] SFX '{name}' tidak ditemukan.");
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource && clip) sfxSource.PlayOneShot(clip);
    }

    public void PlayVoiceOver(string name)
    {
        if (voDict.TryGetValue(name, out var clip))
            PlayVoiceOver(clip);
        else
            Debug.LogWarning($"[AudioManager] Voice Over '{name}' tidak ditemukan.");
    }

    public void PlayVoiceOver(AudioClip clip)
    {
        if (!voiceOverSource || !clip) return;

        voiceOverSource.Stop();
        voiceOverSource.clip = clip;
        voiceOverSource.Play();
    }

    public void StopVoiceOver()
    {
        if (voiceOverSource) voiceOverSource.Stop();
    }
}