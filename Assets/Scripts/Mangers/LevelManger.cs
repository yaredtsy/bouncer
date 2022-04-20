using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManger : MonoBehaviour, GameManger.GameEnded
{
    
    public GameObject ball;

   
    public LevelLineData levelLineData;
    public int currentlevel = 0;


    int leftline = 100;
    Vector3 ballPosition;

    void Awake()
    {
        ball = GetComponentInChildren<Score>().gameObject;
        ballPosition = ball.transform.position;
        GameManger.Instance.AddGameEndedListener(this);
    }

    void ResetLevel()
    {
        if (ball)
            ball.transform.position = ballPosition;
    }

    public int GetMaxLineLength()
    {
        return levelLineData.linelength;
    }

    public Scores getScore(float totalDrawnLength)
    {

        float left = levelLineData.linelength - totalDrawnLength;
        float leftpercentage = left / levelLineData.linelength * 100;

        return levelLineData.getScore(leftpercentage);
    }

    public void OnWin(GameResult gameresult)
    { 

    }

    public void OnLoss()
    {
        ResetLevel();
    }
}

[System.Serializable]
public enum Scores

{
    ONESTART,TWOSTAR,THREESTAR,UNKOWN
}