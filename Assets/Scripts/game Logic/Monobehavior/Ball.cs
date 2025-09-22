using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, GameManger.GameLauncher,IDestroyer
{
    Rigidbody2D rb;
    Vector3 ballPosition;
    TrailRenderer trailRenderer;
    
    [Header("Bounce SFX Settings")]
    public float minBounceSpeed = 1f; // Minimum speed required to play bounce SFX

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
        // Play game lost SFX before triggering loss
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOver();
        }
        
        GameManger.Instance.OnLoss();
    }
    private void OnDisable() {
        GameManger.Instance.RemoveGameLauncherListner(this);
    }
    void ResetLevel(){
        this.gameObject.transform.position= ballPosition;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the ball's current speed
        float currentSpeed = rb.velocity.magnitude;
        
        // Only play bounce SFX if the ball is moving fast enough
        if (currentSpeed >= minBounceSpeed)
        {
            // Check if the collision is with a line (has LineRenderer component)
            if (collision.gameObject.GetComponent<LineRenderer>() != null)
            {
                // This is a line collision - play alternating bounce1/bounce2
                AudioManager.Instance.PlayBounceSFX();
            }
            else
            {
                // This is collision with something else - play bounce3
                AudioManager.Instance.PlayBounce3SFX();
            }
        }
    }
}
