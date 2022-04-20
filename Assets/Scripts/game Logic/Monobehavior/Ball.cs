using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, GameManger.GameLauncher,IDestroyer
{
    Rigidbody2D rb;
    Vector3 ballPosition;
    TrailRenderer trailRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        GameManger.Instance.AddGameLauncherListner(this);
        ballPosition = this.gameObject.transform.position;
        trailRenderer = GetComponent<TrailRenderer>();

        DisableGravity();
    }

    public void EnableGravity()
    {
        rb.isKinematic = false;
        GetComponent<TrailRenderer>().enabled = true;
    }

    public void DisableGravity()
    {
        rb.isKinematic = true;
        GetComponent<TrailRenderer>().enabled = false;
        rb.velocity= new Vector2(0,0);
    }
    public void OnReady(){
        DisableGravity();
    }
    public void OnStart()
    {
        EnableGravity();
    }
    public void OnEnd()
    {
        DisableGravity();
        ResetLevel();
    }

    public void OnClean()
    {
        GameManger.Instance.OnLoss();
    }
    private void OnDisable() {
        GameManger.Instance.RemoveGameLauncherListner(this);

    }
    void ResetLevel(){
        this.gameObject.transform.position= ballPosition;
    }
}
