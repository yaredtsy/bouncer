using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "linesBehavour", menuName = "ScriptableObjects/lines", order = 1)]
public class Linesbehaviour : ScriptableObject
{
    
    public List<LineData> lineDatas;

    public LineData GetLineData(float distance){

        foreach (LineData data in lineDatas)
        {
            if( data.start <= distance && distance < data.end){

                return data;
            }

        }
        return null;
    }
    [System.Serializable]
    public class LineData{
       
        public Color color;
        public float strength;
        public float start;
        public float end=0;
        public float width;
    }


}
