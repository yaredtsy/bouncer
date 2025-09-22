using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Timers;
using UnityEngine.SceneManagement;

public class GameManger : Managers<GameManger>
{
    #region gameManger
    List<GameLauncher> gameLauncher = new List<GameLauncher>();

    public void AddGameLauncherListner(GameLauncher listner)
    {
        if (!gameLauncher.Contains(listner))
            gameLauncher.Add(listner);
    }
    public void RemoveGameLauncherListner(GameLauncher listner)
    {
        if (gameLauncher.Contains(listner))
            gameLauncher.Remove(listner);
    }

    public void TriggerGameStart()
    {
        foreach (GameLauncher gameLauncher in gameLauncher)
        {
            gameLauncher.OnStart();
        }
    }
    public void TriggerGameEnd()
    {
        foreach (GameLauncher gameLauncher in gameLauncher)
        {
            gameLauncher.OnEnd();
        }
        
        // Trigger navigation slide out animation
        if (navAnimator != null)
        {
            navAnimator.SetTrigger("SlideOut");
            navAnimator.ResetTrigger("SlideIn");
        }
    }
    public void TriggerGameOnReady()
    {
        // Reset win/loss flags when starting a new game
        isWinInProgress = false;
        isLossInProgress = false;
        
        foreach (GameLauncher launch in gameLauncher)
        {
            launch.OnReady();
        }
        
        
        if (navAnimator != null && !isWinInProgress)
        {
            navAnimator.SetTrigger("SlideIn");
        }
    }

    public interface GameLauncher
    {
        void OnReady();
        void OnStart();
        void OnEnd();
    }
    #endregion

    #region  gameResult

    List<GameEnded> gameEndedListner = new List<GameEnded>();

    public void AddGameEndedListener(GameEnded listner)
    {
        if (!gameEndedListner.Contains(listner))
            gameEndedListner.Add(listner);
    }
    public void RemoveGameEndedListener(GameEnded listner)
    {
        if (gameEndedListner.Contains(listner))
            gameEndedListner.Remove(listner);
    }

    public void TriggerGameEndedOnWin(GameResult result)
    {
        foreach (GameEnded listner in gameEndedListner)
        {
            listner.OnWin(result);
        }
    }
    public void TriggerGameEndedOnLoss()
    {
        foreach (GameEnded listner in gameEndedListner)
        {
            listner.OnLoss();
        }
    }

    public interface GameEnded
    {
        void OnWin(GameResult game);
        void OnLoss();
    }

    #endregion

    #region draw
    List<OnDrawing> ondrawinglistners = new List<OnDrawing>();

    public void AddOnDrawingListener(OnDrawing listner)
    {
        if (!ondrawinglistners.Contains(listner))
            ondrawinglistners.Add(listner);
    }

    public void RemoveDrawingListener(OnDrawing listner)
    {
        if (ondrawinglistners.Contains(listner))
            ondrawinglistners.Remove(listner);
    }

    public void TriggerOnDrawingStart()
    {
        foreach (OnDrawing listners in ondrawinglistners)
        {
            listners.OnDrawingStart();
        }
    }

    public void TriggerOnDrawingEnd(float length)
    {
        foreach (OnDrawing listners in ondrawinglistners)
        {
            listners.OnDrawingEnd(length);
        }
    }
    public void TriggerOnDrawin(float length)
    {
        foreach (OnDrawing listners in ondrawinglistners)
        {
            listners.OnDrawing(totalDistanceDraw + length);
        }
    }
    public interface OnDrawing
    {
        void OnDrawingStart();
        void OnDrawingEnd(float length);
        void OnDrawing(float length);
    }

    #endregion

    public Animator animator;
    private Animator navAnimator; // Will be found at runtime by tag "Nav_UI"

    public int possibleDistance;
    public GameObject[] levels;
    public float totalDistanceDraw;
    public bool isDebuging;
    public int currentLevelNO = 0;
    GameObject currentlevel;
    LevelManger currentlevelmaneger;
    
    // Guard flags to prevent multiple win/loss calls
    private bool isWinInProgress = false;
    private bool isLossInProgress = false;

    public LevelManger Currentlevelmaneger => currentlevelmaneger;

    private void Awake()
    {
        if (!isDebuging)
            LoadLeveL(0);
    }

    private void FindNavUIAnimator()
    {
        // Find Nav_UI GameObject by tag and get its animator
        GameObject navUI = GameObject.FindWithTag("Nav_UI");
        if (navUI != null)
        {
            navAnimator = navUI.GetComponent<Animator>();
            if (navAnimator == null)
            {
                Debug.LogWarning($"Nav_UI GameObject found but no Animator component attached!");
            }
        }
        else
        {
            Debug.LogWarning($"No GameObject found with tag 'Nav_UI'. Make sure your navigation UI has the correct tag.");
        }
    }

#if UNITY_EDITOR
    public bool win;
    public GameObject winPrefab;
    public GameObject lossPrefab;
    public GameObject trapPrefab;


#endif
    void Start()
    {
        // Find the Nav_UI animator at runtime
        FindNavUIAnimator();
        
        if (animator != null)
        {
            Image childImage = animator.GetComponentInChildren<Image>();
            childImage.color = Color.white;
        }
        if (!isDebuging)
            TriggerGameOnReady();
    }
    public void OnWIn()
    {
        // Prevent multiple win calls
        if (isWinInProgress || isLossInProgress)
        {
            return;
        }
        
        isWinInProgress = true;
        Invoke("Win", 1f);
#if UNITY_EDITOR
        win = true;
#endif
    }

    void Win()
    {
        // Play level complete SFX
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLevelComplete();
        }
        
        GameResult gameResult = new GameResult(){ score= currentlevelmaneger.getScore(totalDistanceDraw),level=currentLevelNO };
        TriggerGameEnd();
        TriggerGameEndedOnWin(gameResult);
    }
    public void OnLoss()
    {
        // Prevent multiple loss calls
        if (isLossInProgress || isWinInProgress)
        {
            return;
        }
        
        isLossInProgress = true;
        animator.SetTrigger("StartFade");
        Invoke("RestartCurrentLevel", .5f);
        TriggerGameEnd();
        TriggerGameEndedOnLoss();
        #if UNITY_EDITOR
            win = false;
        #endif
    }

    public void OnPlayTap()
    {
        // Play button click sound
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (totalDistanceDraw < 0)
        {
            Debug.Log("Please Draw");
        }
        // else
        if (totalDistanceDraw == 0)
        {
            Debug.Log("must draw first");
            return;
        }
        TriggerGameStart();
    }
    public void OnRetrayTap()
    {
        // Play button click sound
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        TriggerGameOnReady();
        totalDistanceDraw = 0;
        TriggerOnDrawin(totalDistanceDraw);
    }

    public void OnHomeTap()
    {
        // Play button click sound
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Start music transition
        StartCoroutine(TransitionToMenuMusic());
    }
    
    private IEnumerator TransitionToMenuMusic()
    {
        // Fade out game music
        if(AudioManager.Instance != null)
        {
            yield return StartCoroutine(AudioManager.Instance.FadeMusic(0f, 1f)); // Fade out over 1 second
        }
        
        // Load the menu scene
        SceneManager.LoadScene(0);
    }
    public void OnNextLevelTap()
    {
        // Play button click sound
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        LoadNextLevel();
        TriggerGameOnReady();
    }

    public bool LoadLevel(GameObject gameObject)
    {
        if (gameObject == null)
        {

            return false;
        }
        if (currentlevel != null)
        {
            Destroy(currentlevel);
            currentlevelmaneger = null;
        }
        GameObject newLevel = Instantiate(gameObject);
        currentlevel = newLevel;
        currentlevelmaneger = currentlevel.GetComponent<LevelManger>();

        totalDistanceDraw = 0;
        TriggerOnDrawin(totalDistanceDraw);

        return true;

    }

    bool LoadLeveL(int level)
    {

        return LoadLevel(levels[level]);
    }
    bool LoadNextLevel()
    {
      
        currentLevelNO++;
     

        return LoadLevel(levels[currentLevelNO]);
    }
    void RestartCurrentLevel()
    {
        // Reset win/loss flags when restarting
        isWinInProgress = false;
        isLossInProgress = false;
        
        // Reload the current level
        LoadLevel(levels[currentLevelNO]);
        
        // Reset game state and trigger ready
        TriggerGameOnReady();
    }
    public void AddDistance(float distance)
    {
        totalDistanceDraw += distance;
        TriggerOnDrawingEnd(totalDistanceDraw);
    }
}

public struct GameResult
{
    public Scores score;
    public int level;

}