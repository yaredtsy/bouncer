using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
 
    void OnTriggerEnter2D(Collider2D other)
    {
        IGoal igoal = other.gameObject.GetComponent<IGoal>();

        if(igoal!=null)
            igoal.OnScore();
    }

    IEnumerator OnScore(IGoal goal){
        yield return new WaitForSeconds(5);
        goal.OnScore();
        yield return null;
    }
}
