using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGame : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject menuCanvas, mainMenu, optionsMenu, selectLevel, Logo, Tree1, Tree2;
    public GameObject crossFade;
    [Header("Transition Settings")]
    [SerializeField] private float fadeDuration = 0.5f; // Configurable fade duration
    private Animator animator;
    private bool isTransitioning = false; // Prevent multiple transitions
    
    // Static variable to track if logo has been shown in current session
    private static bool hasShownLogoThisSession = false;

    private void Awake()
    {
        if(menuCanvas != null)
        menuCanvas.SetActive(false);
    }
    void Start()
    {
        if (crossFade != null)
            animator = crossFade.GetComponentInChildren<Animator>();
        selectLevel.SetActive(false);
        optionsMenu.SetActive(false);

        // Check if we should show the logo video
        if (!hasShownLogoThisSession && videoPlayer != null)
        {
            // First time in this session - show logo video
            hasShownLogoThisSession = true;
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
    
    private void OnVideoFinished(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);
        menuCanvas.SetActive(true);
    }

    public void onPlayClicked()
    {
        if (isTransitioning) return; // Prevent multiple clicks during transition
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

    public void onLevelClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void onOptionsClicked()
    {
        if (isTransitioning) return;
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
        Application.Quit();
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