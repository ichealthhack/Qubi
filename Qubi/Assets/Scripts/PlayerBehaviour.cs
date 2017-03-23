using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float JumpForce = 1;
    public float PlayerSpeed = 20f;
    public float Gravity = 9.8f;

    private Rigidbody2D thisRigidbody;

    private AudioSource JumpEffect;

    // Use this for initialization
    void Start()
    {
        JumpEffect = this.GetComponent<AudioSource>();
        thisRigidbody = this.GetComponent<Rigidbody2D>();
        thisRigidbody.gravityScale = Gravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.Instance.LevelRunning && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        //Vector2 newPosition = new Vector2(this.transform.position.x * PlayerSpeed * Time.deltaTime, this.transform.position.y);
        //thisRigidbody.MovePosition(newPosition);
        if (ScoreManager.Instance.LevelRunning)
        {
            float levelProgress = (float)ScoreManager.Instance.CurrentLevel.ExhalationCount / (float)ScoreManager.Instance.CurrentLevel.ExhalationMax;
            PlayerSpeed = Mathf.Lerp(ScoreManager.Instance.CurrentLevel.MinPlayerSpeed, ScoreManager.Instance.CurrentLevel.MaxPlayerSpeed, levelProgress);

            thisRigidbody.velocity = new Vector2(PlayerSpeed, thisRigidbody.velocity.y);
        }
        else
        {
            thisRigidbody.velocity = Vector2.zero;
        }
    }

    void Jump()
    {
        JumpEffect.Stop();
        JumpEffect.Play();
        thisRigidbody.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
    }
}
