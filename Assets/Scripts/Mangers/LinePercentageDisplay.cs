using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinePercentageDisplay : MonoBehaviour, GameManger.OnDrawing
{
    public Text linelengthpercent;
    public Slider linelengthslider;

    private void Start() {
        GameManger.Instance.AddOnDrawingListener(this);
    }
    
    public void OnDrawingStart(){

    }

    public void OnDrawingEnd(float length)
    {
    }

    public void OnDrawing(float length)
    {
 

        float left = GameManger.Instance.Currentlevelmaneger.GetMaxLineLength() - length;
        float LevelLineData = left * 100 / GameManger.Instance.Currentlevelmaneger.GetMaxLineLength();

        linelengthpercent.text =$"{(int)LevelLineData}%";
        linelengthslider.value = LevelLineData / 100;
    }
}
