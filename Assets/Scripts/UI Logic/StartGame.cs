using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGame : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject menuCanvas, mainMenu, optionsMenu, selectLevel, Logo, Tree1, Tree2;
    public GameObject crossFade;
    
    [Header("Mute Button Sprites")]
    public UnityEngine.UI.Button musicButton;
    public Sprite musicMutedSprite;    // UI_Button_27
    public Sprite musicUnmutedSprite;  // UI_Button_9
    
    public UnityEngine.UI.Button sfxButton;
    public Sprite sfxMutedSprite;      // UI_Button_25
    public Sprite sfxUnmutedSprite;    // UI_Button_6
    [Header("Transition Settings")]
    [SerializeField] private float fadeDuration = 0.5f; // Configurable fade duration
    private Animator animator;
    private bool isTransitioning = false; // Prevent multiple transitions
    
    // Static variable to track if logo has been shown in current session
    public static bool hasShownLogoThisSession = false;

    //public static StartGame Instance;

    private void Awake()
    {
       // Instance = this;
        if(menuCanvas != null)
        menuCanvas.SetActive(false);
    }
    void Start()
    {
        if (progressResetPanel != null)
            progressResetPanel.SetActive(false);
        if (AboutPanel != null)
            AboutPanel.SetActive(false);

        if (crossFade != null)
            animator = crossFade.GetComponentInChildren<Animator>();
        selectLevel.SetActive(false);
        optionsMenu.SetActive(false);

        UpdateMuteButtonSprites();
        // Check if we should show the logo video
        if (!hasShownLogoThisSession && videoPlayer != null)
        {
            // First time in this session - show logo video
            hasShownLogoThisSession = true;
            
            // Setup video event to play audio when video starts
            videoPlayer.started += OnVideoStarted;
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.gameObject.SetActive(true);
        }
        else
        {
            // Logo already shown or no video player - skip to main menu
            if (videoPlayer != null)
                videoPlayer.gameObject.SetActive(false);
            menuCanvas.SetActive(true);
        }
    }
    
    private void OnVideoStarted(VideoPlayer vp)
    {
        // Play splash audio when video actually starts
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySplashVideo();
        }
    }
    
    private void OnVideoFinished(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);
        menuCanvas.SetActive(true);
        
        // Start main menu music
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMainMenuMusic();
        }
    }

    public void onPlayClicked()
    {
        if (isTransitioning) return; // Prevent multiple clicks during transition
        
        // Play button click sound
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        StartCoroutine(TransitionToLevelSelect());
    }

    private IEnumerator TransitionToLevelSelect()
    {
        isTransitioning = true;
        
        if(animator != null)
            animator.SetTrigger("StartFade");

        // Wait for fade animation to complete (adjust time based on your animation)
        yield return new WaitForSeconds(fadeDuration); // Now uses configurable duration

        // Now switch UI elements
        Logo.SetActive(false);
        Tree1.SetActive(false);
        Tree2.SetActive(false);
        mainMenu.SetActive(false);
        selectLevel.SetActive(true);
        
        isTransitioning = false;
    }

    public void OnBackClicked()
    {
        if (isTransitioning) return;

        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        if (progressResetPanel != null)
            progressResetPanel.SetActive(false);
        if (AboutPanel != null)
            AboutPanel.SetActive(false);
        }
        
        StartCoroutine(TransitionToMainMenu());
    }

    private IEnumerator TransitionToMainMenu()
    {
        isTransitioning = true;
        
        if (animator != null)
            animator.SetTrigger("StartFade");

        // Wait for fade animation to complete
        yield return new WaitForSeconds(fadeDuration); // Now uses configurable duration

        // Now switch UI elements
        Logo.SetActive(true);
        Tree1.SetActive(true);
        Tree2.SetActive(true);
        selectLevel.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        
        isTransitioning = false;
    }

    public void onOptionsClicked()
    {
        if (isTransitioning) return;
        
        // Play button click sound
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        StartCoroutine(TransitionToOptions());
    }

    private IEnumerator TransitionToOptions()
    {
        isTransitioning = true;
        
        if (animator != null)
            animator.SetTrigger("StartFade");

        // Wait for fade animation to complete
        yield return new WaitForSeconds(fadeDuration); // Now uses configurable duration

        // Now switch UI elements
        Logo.SetActive(false);
        Tree1.SetActive(false);
        Tree2.SetActive(false);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        
        isTransitioning = false;
    }

    public void onQuitClicked()
    {
        // Play button click sound
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        Application.Quit();
    }
    
    // Music toggle method
    public void onMusicToggleClicked()
    {
        if (progressResetPanel != null)
            progressResetPanel.SetActive(false);
        if (AboutPanel != null)
            AboutPanel.SetActive(false);
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Toggle music mute state
        if(AudioManager.Instance != null && musicButton != null)
        {
            AudioManager.Instance.ToggleMusicMute();
            
            // Update button sprite based on mute state
            if(AudioManager.Instance.isMusicMuted)
            {
                musicButton.image.sprite = musicMutedSprite;
            }
            else
            {
                musicButton.image.sprite = musicUnmutedSprite;
            }
        }
    }
    
    // SFX toggle method
    public void onSFXToggleClicked()
    {
        if (progressResetPanel != null)
            progressResetPanel.SetActive(false);
        if (AboutPanel != null)
            AboutPanel.SetActive(false);
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Toggle SFX mute state
        if(AudioManager.Instance != null && sfxButton != null)
        {
            AudioManager.Instance.ToggleSFXMute();
            
            // Update button sprite based on mute state
            if(AudioManager.Instance.isSFXMuted)
            {
                sfxButton.image.sprite = sfxMutedSprite;
            }
            else
            {
                sfxButton.image.sprite = sfxUnmutedSprite;
            }
        }
    }
    
    // Progress button method
    public GameObject progressResetPanel; // Assign in inspector
    public GameObject AboutPanel;         // Assign in inspector

    public void onProgressClicked()
    {
        // Play button click sound
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        // Hide AboutPanel if visible
        if (AboutPanel != null)
            AboutPanel.SetActive(false);

        // Show progressResetPanel
        if (progressResetPanel != null)
            progressResetPanel.SetActive(true);
    }

    public void onResetyesClicked()
    {
        // Play button click sound
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Reset all PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Hide progressResetPanel
        if (progressResetPanel != null)
            progressResetPanel.SetActive(false);
    }

    public void onResetnoClicked()
    {
        // Play button click sound
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        // Hide progressResetPanel
        if (progressResetPanel != null)
            progressResetPanel.SetActive(false);
    }
    
    // Language button method
    public void onLanguageClicked()
    {
        // Play button click sound
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        // Hide progressResetPanel
        if (progressResetPanel != null)
            progressResetPanel.SetActive(false);

        // Show AboutPanel
        if (AboutPanel != null)
            AboutPanel.SetActive(true);
    }

    // Update button sprites based on current mute states
    private void UpdateMuteButtonSprites()
    {
        if(AudioManager.Instance != null)
        {
            // Update music button sprite
            if(musicButton != null)
            {
                if(AudioManager.Instance.isMusicMuted)
                {
                    musicButton.image.sprite = musicMutedSprite;
                }
                else
                {
                    musicButton.image.sprite = musicUnmutedSprite;
                }
            }
            
            // Update SFX button sprite
            if(sfxButton != null)
            {
                if(AudioManager.Instance.isSFXMuted)
                {
                    sfxButton.image.sprite = sfxMutedSprite;
                }
                else
                {
                    sfxButton.image.sprite = sfxUnmutedSprite;
                }
            }
        }
    }
    
    // Public method to reset logo state (useful for testing or game restart)
    public void ResetLogoState()
    {
        hasShownLogoThisSession = false;
    }
    
    // Call this when starting a new game session
    public void OnNewGameSession()
    {
        hasShownLogoThisSession = false;
    }
}