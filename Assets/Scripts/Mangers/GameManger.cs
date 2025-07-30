using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Timers;

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
    }
    public void TriggerGameOnReady()
    {
        foreach (GameLauncher launch in gameLauncher)
        {
            launch.OnReady();
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

    public int possibleDistance;
    public GameObject[] levels;
    public float totalDistanceDraw;
    public bool isDebuging;
    public int currentLevelNO = 0;
    GameObject currentlevel;
    LevelManger currentlevelmaneger;

    public LevelManger Currentlevelmaneger => currentlevelmaneger;

    private void Awake()
    {
        if (!isDebuging)
            LoadLeveL(0);
    }

#if UNITY_EDITOR
    public bool win;
    public GameObject winPrefab;
    public GameObject lossPrefab;
    public GameObject trapPrefab;


#endif
    void Start()
    {
        if (!isDebuging)
            TriggerGameOnReady();
    }
    public void OnWIn()
    {
        Invoke("Win", 1f);
#if UNITY_EDITOR
        win = true;
#endif
    }

    void Win()
    {
        GameResult gameResult = new GameResult(){ score= currentlevelmaneger.getScore(totalDistanceDraw),level=currentLevelNO };
        TriggerGameEnd();
        TriggerGameEndedOnWin(gameResult);
    }
    public void OnLoss()
    {
        animator.SetTrigger("StartFade");
        TriggerGameEnd();
        TriggerGameEndedOnLoss();
        Invoke("RestartCurrentLevel", .5f);
        #if UNITY_EDITOR
                 win = false;
        #endif
    }

    public void OnPlayTap()
    {
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
        TriggerGameOnReady();
        totalDistanceDraw = 0;
        TriggerOnDrawin(totalDistanceDraw);
    }
    public void OnNextLevelTap()
    {
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