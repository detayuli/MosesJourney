using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class SceneMusicMapping
    {
        public string musicName;
        public EventReference musicEvent;
        public List<string> sceneNames;
    }

    [Header("Scene BGM Setup")]
    [SerializeField] private List<SceneMusicMapping> sceneMusicMappings = new();

    private readonly Dictionary<string, EventReference> sceneMusicDict = new();
    private EventInstance currentMusicInstance;
    private EventInstance currentVOInstance;
    private EventReference currentMusicEvent;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeMusicDictionary();
        SceneManager.activeSceneChanged += OnSceneChanged;

        // Panggil langsung di Awake biar scene pertama langsung bunyi
        PlayMusicInCurrentScene();
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
        StopMusic();
        StopVoiceOver();
    }

    void InitializeMusicDictionary()
    {
        foreach (var m in sceneMusicMappings)
        {
            if (m.musicEvent.IsNull) continue;

            foreach (var scene in m.sceneNames)
                if (!string.IsNullOrEmpty(scene))
                    sceneMusicDict[scene] = m.musicEvent;
        }
    }

    void OnSceneChanged(Scene _, Scene __) => PlayMusicInCurrentScene();

    public void PlayMusicInCurrentScene()
    {
        string scene = SceneManager.GetActiveScene().name;

        if (sceneMusicDict.TryGetValue(scene, out var musicEvent))
        {
            if (currentMusicEvent.Path == musicEvent.Path && currentMusicInstance.isValid()) return;

            StopMusic();

            currentMusicEvent = musicEvent;
            currentMusicInstance = RuntimeManager.CreateInstance(musicEvent);
            currentMusicInstance.start();
        }
        else
        {
            StopMusic();
        }
    }

    public void StopMusic()
    {
        if (currentMusicInstance.isValid())
        {
            currentMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusicInstance.release();
            currentMusicEvent = default;
        }
    }

    public void PlaySFX(string eventPath)
    {
        if (!string.IsNullOrEmpty(eventPath))
            RuntimeManager.PlayOneShot(eventPath);
    }

    public void PlaySFX(EventReference sfxEvent)
    {
        if (!sfxEvent.IsNull)
            RuntimeManager.PlayOneShot(sfxEvent);
    }

    public void PlayVoiceOver(string eventPath)
    {
        if (string.IsNullOrEmpty(eventPath)) return;

        StopVoiceOver();
        currentVOInstance = RuntimeManager.CreateInstance(eventPath);
        currentVOInstance.start();
    }

    public void PlayVoiceOver(EventReference voEvent)
    {
        if (voEvent.IsNull) return;

        StopVoiceOver();
        currentVOInstance = RuntimeManager.CreateInstance(voEvent);
        currentVOInstance.start();
    }

    public void StopVoiceOver()
    {
        if (currentVOInstance.isValid())
        {
            currentVOInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentVOInstance.release();
        }
    }
}