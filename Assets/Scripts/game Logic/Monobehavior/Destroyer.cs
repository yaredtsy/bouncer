using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D other)
    {
        IDestroyer Idestroyer =other.gameObject.GetComponent<IDestroyer>();
        if(Idestroyer!=null){
            Idestroyer.OnClean();
        }
    }
}
