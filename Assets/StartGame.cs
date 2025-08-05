using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer not assigned! Please assign it in the Inspector.");
            return;
        }
        videoPlayer.loopPointReached += OnVideoFinished;
    }
    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(1);
    }
}