using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;
    
    [Header("SFX Clips")]
    public AudioClip buttonClickSFX;
    public AudioClip splashVideoSFX;
    public AudioClip levelCompleteSFX;
    public AudioClip gameOverSFX;
    
    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    
    [Header("Mute States")]
    public bool isMusicMuted = false;
    public bool isSFXMuted = false;
    
    private void Awake()
    {
        // Singleton pattern - only one AudioManager can exist
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize audio sources if not assigned
            if (musicSource == null)
                musicSource = gameObject.AddComponent<AudioSource>();
            if (sfxSource == null)
                sfxSource = gameObject.AddComponent<AudioSource>();
            
            // Configure audio sources
            SetupAudioSources();
            
            // Listen for scene changes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we're in the game scene (scene index 1)
        if (scene.buildIndex == 1 && isMusicMuted == false)
        {
            // Start game music with fade in
            StartCoroutine(StartGameMusicWithFade());
        }
        // Check if we're in the menu scene (scene index 0)
        else if (scene.buildIndex == 0 && StartGame.hasShownLogoThisSession == true && isMusicMuted == false)
        {   
            StartCoroutine(StartMenuMusicWithFade());
        }
    }
    
    private IEnumerator StartGameMusicWithFade()
    {
        // Play game music
        PlayGameMusic();
        
        // Fade in the game music over 2 seconds
        yield return StartCoroutine(FadeMusic(0.7f, 2f));
    }
    
    private IEnumerator StartMenuMusicWithFade()
    {
        // Play menu music
        PlayMainMenuMusic();
        
        // Fade in the menu music over 2 seconds
        yield return StartCoroutine(FadeMusic(0.7f, 2f));
    }
    
    private void SetupAudioSources()
    {
        // Configure music source
        musicSource.loop = true;
        musicSource.volume = isMusicMuted ? 0f : musicVolume;
        musicSource.playOnAwake = false;
        
        // Configure SFX source for low latency
        sfxSource.loop = false;
        sfxSource.volume = isSFXMuted ? 0f : sfxVolume;
        sfxSource.playOnAwake = false;
        sfxSource.priority = 0; // Highest priority
        sfxSource.bypassEffects = true; // Bypass audio effects for faster playback
        sfxSource.bypassListenerEffects = true;
        sfxSource.bypassReverbZones = true;
    }
    
    // Music Methods
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
    }
    
    public void PauseMusic()
    {
        musicSource.Pause();
    }
    
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
    
    // SFX Methods
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    public void PlaySFX(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
    
    // Low-latency SFX method for UI sounds
    public void PlayUISFX(AudioClip clip)
    {
        if (clip != null)
        {
            // Stop any currently playing SFX to avoid overlap
            sfxSource.Stop();
            sfxSource.PlayOneShot(clip);
        }
    }
    
    // Convenience methods for common sounds
    public void PlayButtonClick()
    {
        PlayUISFX(buttonClickSFX);
    }
    
    public void PlaySplashVideo()
    {
        PlaySFX(splashVideoSFX);
    }
    
    public void PlayLevelComplete()
    {
        PlaySFX(levelCompleteSFX);
    }
    
    public void PlayGameOver()
    {
        PlaySFX(gameOverSFX);
    }
    
    // Volume Control Methods
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = isMusicMuted ? 0f : musicVolume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = isSFXMuted ? 0f : sfxVolume;
    }
    
    // Mute Toggle Methods
    public void ToggleMusicMute()
    {
        isMusicMuted = !isMusicMuted;
        musicSource.volume = isMusicMuted ? 0f : musicVolume;
    }
    
    public void ToggleSFXMute()
    {
        isSFXMuted = !isSFXMuted;
        sfxSource.volume = isSFXMuted ? 0f : sfxVolume;
    }
    
    // Scene-specific music methods
    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic);
    }
    
    public void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }
    
    // Fade music in/out
    public IEnumerator FadeMusic(float targetVolume, float fadeTime)
    {
        float startVolume = musicSource.volume;
        float currentTime = 0;
        
        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeTime);
            yield return null;
        }
        
        musicSource.volume = targetVolume;
    }
}
