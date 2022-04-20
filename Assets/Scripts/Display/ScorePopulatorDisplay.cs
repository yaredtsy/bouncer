using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScorePopulatorDisplay : MonoBehaviour, GameManger.GameLauncher
{
    public List<Text> scoreTexts;
    public int startingpositionx = 0;
    public int endingpostionx = 400;

    public int yposition = -40;
    Transform inittransform;
    Vector3 initialpostion;

    int totallength = 0;

    void Awake()
    {
        GameManger.Instance.AddGameLauncherListner(this);
        totallength = 400;
        
        inittransform = scoreTexts[0].gameObject.transform;
        initialpostion = inittransform.localPosition;
        Debug.Log("Start");
    }
    public void OnEnd()
    {

    }

    public void OnReady()
    {
        Debug.Log("Onready");
        bool changed = false;
        foreach (LevelLineData.ScoreData scoreData in GameManger.Instance.Currentlevelmaneger.levelLineData.scoresData)
        {
            switch (scoreData.score)
            {
                case Scores.ONESTART:
                    Text scoreText = scoreTexts[0];
                    Transform trans = scoreText.gameObject.transform;
                    scoreText.text = "|";

                    float x = totallength * scoreData.percentage / 100;

                    Vector3 newpos = new Vector3(initialpostion.x+x, trans.localPosition.y, trans.localPosition.z);
                    trans.localPosition = newpos;
            
                    break;
                case Scores.TWOSTAR:
                    Text scoreText2 = scoreTexts[1];
                    Transform transform2 = scoreText2.gameObject.transform;
                    scoreText2.text = "|";

                    float x2 = totallength * scoreData.percentage / 100;
                    transform2.localPosition = new Vector3(initialpostion.x+x2, inittransform.localPosition.y, inittransform.localPosition.z);
                    break;
                case Scores.THREESTAR:
                    Text scoreTexts3 = scoreTexts[2];
                    Transform transform3 = scoreTexts3.gameObject.transform;
                    scoreTexts3.text = "|";

                    float x3 = totallength * scoreData.percentage / 100;
                    transform3.localPosition = new Vector3(initialpostion.x+x3, inittransform.localPosition.y, inittransform.localPosition.z);
                    break;
                default:
                    return;
            }
        }
     

    }
    void Correctposition()
    {
        foreach(Text text in scoreTexts)
        {
            Vector3 pos = text.gameObject.transform.position;
            text.gameObject.transform.position = new Vector3(pos.x+300,pos.y,pos.z);
        }
    }
    public void OnStart()
    {
        
    }
    
}
