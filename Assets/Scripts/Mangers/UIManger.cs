using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : MonoBehaviour, GameManger.GameLauncher, GameManger.GameEnded
{
    private Transition _transition;
    public GameObject playUI;

    public GameObject winUI;
    public Text level,level2;


    // public static UIManger Getinstance => instance;

    void Start()
    {
        OnReady();
        GameManger.Instance.AddGameLauncherListner(this);
        GameManger.Instance.AddGameEndedListener(this);
    }

    public void OnWin(GameResult game)
    {
        winUI.SetActive(true);
        playUI.SetActive(false);
    }

    public void OnLoss()
    {
        winUI.SetActive(false);
        playUI.SetActive(false);
    }

    public void OnReady()
    {
        level.text = "Level " + GameManger.Instance.Currentlevelmaneger.currentlevel;
        level2.text = "Level " + GameManger.Instance.Currentlevelmaneger.currentlevel;
        winUI.SetActive(false);
        playUI.SetActive(true);
    }

    public void OnStart()
    {
        playUI.SetActive(false);
    }

    public void OnEnd()
    {

    }


}
