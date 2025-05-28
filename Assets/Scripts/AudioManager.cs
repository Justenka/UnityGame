using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource playerAudioSource;

    public string ambientAudioObjectName = "Map";
    private AudioSource _ambientAudioSource;

    public string enemyTag = "Enemy";

    private List<AudioSource> enemyAudioSources = new List<AudioSource>();
    private Dictionary<AudioSource, float> originalVolumes = new Dictionary<AudioSource, float>();

    private float playerVolumeModifier = 1.0f;
    private float enemyVolumeModifier = 1.0f;
    private float ambientVolumeModifier = 1.0f;

    private const string PLAYER_VOLUME_KEY = "PlayerVolumeModifier";
    private const string ENEMY_VOLUME_KEY = "EnemyVolumeModifier";
    private const string AMBIENT_VOLUME_KEY = "AmbientVolumeModifier";

    public Slider playerVolumeSlider;
    public Slider enemyVolumeSlider;
    public Slider ambientVolumeSlider;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadVolumeModifiers();

        CacheOriginalVolume(playerAudioSource);
        FindAmbientAudioSource();
        FindEnemyAudioSources();

        ApplyAllVolumeModifiers();

        UpdateUISliderValues();

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        FindAmbientAudioSource();
        FindEnemyAudioSources();
        ApplyAllVolumeModifiers();
        UpdateUISliderValues();
    }

    private void FindAmbientAudioSource()
    {
        if (string.IsNullOrEmpty(ambientAudioObjectName))
        {
            _ambientAudioSource = null;
            return;
        }

        GameObject ambientGO = GameObject.Find(ambientAudioObjectName);
        if (ambientGO != null)
        {
            _ambientAudioSource = ambientGO.GetComponent<AudioSource>();
            if (_ambientAudioSource != null)
            {
                CacheOriginalVolume(_ambientAudioSource);
            }
        }
        else
        {
            _ambientAudioSource = null;
        }
    }

    private void FindEnemyAudioSources()
    {
        List<AudioSource> sourcesToRemove = new List<AudioSource>();
        foreach (var entry in originalVolumes)
        {
            if (entry.Key != playerAudioSource && entry.Key != _ambientAudioSource)
            {
                if (entry.Key == null || entry.Key.gameObject == null)
                {
                    sourcesToRemove.Add(entry.Key);
                }
            }
        }
        foreach (var source in sourcesToRemove)
        {
            originalVolumes.Remove(source);
            enemyAudioSources.Remove(source);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        enemyAudioSources.Clear();
        foreach (GameObject enemy in enemies)
        {
            AudioSource enemyAS = enemy.GetComponent<AudioSource>();
            if (enemyAS != null)
            {
                enemyAudioSources.Add(enemyAS);
                CacheOriginalVolume(enemyAS);
            }
        }
    }

    private void CacheOriginalVolume(AudioSource source)
    {
        if (source != null && !originalVolumes.ContainsKey(source))
        {
            originalVolumes.Add(source, source.volume);
        }
    }

    public void AddEnemyAudioSource(AudioSource newEnemyAS)
    {
        if (newEnemyAS != null && !enemyAudioSources.Contains(newEnemyAS))
        {
            enemyAudioSources.Add(newEnemyAS);
            CacheOriginalVolume(newEnemyAS);
            ApplyModifier(newEnemyAS, enemyVolumeModifier);
        }
    }

    public void RemoveEnemyAudioSource(AudioSource enemyAS)
    {
        if (enemyAS != null)
        {
            enemyAudioSources.Remove(enemyAS);
            if (originalVolumes.ContainsKey(enemyAS))
            {
                originalVolumes.Remove(enemyAS);
            }
        }
    }

    private void ApplyModifier(AudioSource source, float modifier)
    {
        if (source != null && originalVolumes.ContainsKey(source))
        {
            source.volume = originalVolumes[source] * modifier;
        }
        else if (source != null)
        {
            source.volume *= modifier;
        }
    }

    private void ApplyAllVolumeModifiers()
    {
        ApplyModifier(playerAudioSource, playerVolumeModifier);
        ApplyModifier(_ambientAudioSource, ambientVolumeModifier);
        foreach (AudioSource enemyAS in enemyAudioSources)
        {
            ApplyModifier(enemyAS, enemyVolumeModifier);
        }
    }

    public void SetPlayerVolumeModifier(float sliderValue)
    {
        playerVolumeModifier = sliderValue;
        ApplyModifier(playerAudioSource, playerVolumeModifier);
        PlayerPrefs.SetFloat(PLAYER_VOLUME_KEY, playerVolumeModifier);
    }

    public void SetEnemyVolumeModifier(float sliderValue)
    {
        enemyVolumeModifier = sliderValue;
        foreach (AudioSource enemyAS in enemyAudioSources)
        {
            ApplyModifier(enemyAS, enemyVolumeModifier);
        }
        PlayerPrefs.SetFloat(ENEMY_VOLUME_KEY, enemyVolumeModifier);
    }

    public void SetAmbientVolumeModifier(float sliderValue)
    {
        ambientVolumeModifier = sliderValue;
        ApplyModifier(_ambientAudioSource, ambientVolumeModifier);
        PlayerPrefs.SetFloat(AMBIENT_VOLUME_KEY, ambientVolumeModifier);
    }

    private void LoadVolumeModifiers()
    {
        playerVolumeModifier = PlayerPrefs.GetFloat(PLAYER_VOLUME_KEY, 1.0f);
        enemyVolumeModifier = PlayerPrefs.GetFloat(ENEMY_VOLUME_KEY, 1.0f);
        ambientVolumeModifier = PlayerPrefs.GetFloat(AMBIENT_VOLUME_KEY, 1.0f);
    }

    private void UpdateUISliderValues()
    {
        if (playerVolumeSlider != null)
        {
            playerVolumeSlider.value = playerVolumeModifier;
        }
        if (enemyVolumeSlider != null)
        {
            enemyVolumeSlider.value = enemyVolumeModifier;
        }
        if (ambientVolumeSlider != null)
        {
            ambientVolumeSlider.value = ambientVolumeModifier;
        }
    }

    public void SaveAllVolumeSettings()
    {
        PlayerPrefs.Save();
    }
}