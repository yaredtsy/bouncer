using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : MonoBehaviour, GameManger.GameLauncher, GameManger.GameEnded
{
    private Transition _transition;
    public GameObject playUI;

    public GameObject winUI;
    public GameObject retrayUI;
    public Text level;


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
        retrayUI.SetActive(false);
    }

    public void OnLoss()
    {
        Debug.Log("OnLoss UIManger");
        winUI.SetActive(false);
        playUI.SetActive(false);
        //retrayUI.SetActive(true);
    }

    public void OnReady()
    {
        Debug.Log("OnReady UIManger");
        level.text = "Level " + GameManger.Instance.Currentlevelmaneger.currentlevel;
        winUI.SetActive(false);
        playUI.SetActive(true);
        retrayUI.SetActive(false);
    }

    public void OnStart()
    {
        playUI.SetActive(false);
    }

    public void OnEnd()
    {

    }


}
