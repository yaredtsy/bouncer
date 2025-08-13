using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject menuCanvas, mainMenu, optionsMenu, selectLevel, Logo, Tree1, Tree2;
    public GameObject crossFade;
    private Animator animator;

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

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer not assigned! Please assign it in the Inspector.");
            return;
        }
        videoPlayer.loopPointReached += OnVideoFinished;
    }
    private void OnVideoFinished(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);
        menuCanvas.SetActive(true);
    }

    public void onPlayClicked()
    {
        if(animator != null)
            animator.SetTrigger("StartFade");

        Logo.SetActive(false);
        Tree1.SetActive(false);
        Tree2.SetActive(false);
        mainMenu.SetActive(false);
        selectLevel.SetActive(true);
    }

    public void OnBackClicked()
    {
        if (animator != null)
            animator.SetTrigger("StartFade");

        Logo.SetActive(true);
        Tree1.SetActive(true);
        Tree2.SetActive(true);
        selectLevel.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void onLevelClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void onOptionsClicked()
    {
        if (animator != null)
            animator.SetTrigger("StartFade");

        Logo.SetActive(false);
        Tree1.SetActive(false);
        Tree2.SetActive(false);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void onQuitClicked()
    {
        Application.Quit();
    }
}