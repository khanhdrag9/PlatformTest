using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float decelaration;
    public float turnSpeed;
    public float airMaxSpeed;
    public float airAcceleration;
    public float airDecelaration;
    public float airTurnSpeed;
    public bool instantMovement;
    public float maxSpeedScale = 1;

    float _moveSpeed;
    float _speedChange;
    Vector2 _velocity;
    float _directionX;
    bool _pressingKey;
    Vector2 _desiredVelocity;
    bool _isOnGround;

    Rigidbody2D _rb;
    PlayerGround _playerGround;



    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerGround = GetComponent<PlayerGround>();
    }

    void Update()
    {
        InputCheck();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        _isOnGround = _playerGround.isOnGround;
        _desiredVelocity = new Vector2(_directionX, 0f) * (_isOnGround ? maxSpeed : airMaxSpeed) * maxSpeedScale;
        _velocity = _rb.velocity;

        if (instantMovement)
        {
            _velocity.x = _desiredVelocity.x;
            _rb.velocity = _velocity;
        }
        else
        {
            var a = _isOnGround ? acceleration : airAcceleration;
            var da = _isOnGround ? decelaration : airDecelaration;
            var t = _isOnGround ? turnSpeed : airTurnSpeed;

            if (_pressingKey)
            {
                if (Mathf.Sign(_directionX) != Mathf.Sign(_velocity.x))
                {
                    _speedChange = t * Time.fixedDeltaTime;
                }
                else
                {
                    _speedChange = a * Time.fixedDeltaTime;
                }
            }
            else
            {
                _speedChange = da * Time.fixedDeltaTime;
            }

            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _speedChange);

            _rb.velocity = _velocity;
        }
    }

    private void InputCheck()
    {
        _directionX = Input.GetAxis("Horizontal");
        if (_directionX != 0)
        {
            transform.localScale = new Vector3(_directionX > 0 ? 1 : -1, 1, 1);
            _pressingKey = true;
        }
        else
        {
            _pressingKey = false;
        }
    }
}
