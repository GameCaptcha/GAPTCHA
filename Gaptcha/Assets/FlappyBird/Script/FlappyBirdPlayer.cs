using System;
using UnityEngine;

public class FlappyBirdPlayer : MonoBehaviour
{
    private float _jumpPower = 4; // 점프 세기

    private bool _isDie = false;
    private bool _jumpPressed;
    
    [SerializeField] Rigidbody2D myRigidbody;
    
    void Start() 
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init()
    {
        _isDie = false;
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _jumpPressed = true;
    }

    private void FixedUpdate() 
    {
        if (_jumpPressed) 
        {
            myRigidbody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            transform.rotation = Quaternion.Euler(0, 0, 15f);
            
            _jumpPressed = false;
        }

        if (myRigidbody.linearVelocity.y < -.5f && myRigidbody.linearVelocity.y > -4f) 
        {
            transform.Rotate(0, 0, -1.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        Debug.Log("GameOver");
        _isDie = true;

        GlobalGameManager.Instance.GameOver();
    } // 충돌 시 게임 정지 
}
