using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour,GameManger.GameEnded
{
    public Text score;
    public List<GameObject> stars;
    GameResult gameResult;
    public void OnLoss()
    {
        
    }


    public void OnWin(GameResult gameResult)
    {
        switch (gameResult.score)
        {
            case Scores.ONESTART:
                Display(1);
                break;
            case Scores.TWOSTAR:
                Display(2);
             
                break;
            case Scores.THREESTAR:
                Display(3);
                
                break;
            default:
                break;
        }
    }
    void Display(int amount)
    {
        for (int i = 0; i < stars.Count; i++)
        {
            if (i < amount)
                stars[i].gameObject.SetActive(true);
            else
                stars[i].gameObject.SetActive(false);
          
        }
    }
    IEnumerator DisplayStars(int amount)
    {
        for (int i = 0; i < stars.Count; i++)
        {
            if (i < amount)
                stars[i].gameObject.SetActive(true);
            else
                stars[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManger.Instance.AddGameEndedListener(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
