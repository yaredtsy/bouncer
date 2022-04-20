using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour,IGoal
{
    public void OnScore()
    {
        GameManger.Instance.OnWIn();

    }
  
}
