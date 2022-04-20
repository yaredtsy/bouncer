using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "levellinedata", menuName = "ScriptableObjects/leveldata", order = 2)]
public class LevelLineData : ScriptableObject
{
    public int linelength = 10;
    
    public List<ScoreData> scoresData;

    public Scores getScore(float percentage)
    {
        foreach(ScoreData scoredata in scoresData)
        {
            if(scoredata.percentage <= percentage)
            {
                return scoredata.score;
            }
        }
        return Scores.UNKOWN;
    }

    [System.Serializable]
    public class ScoreData
    {
        public Scores score;
        public float percentage;
    }
}
