using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
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
        UpdateMovement();

    }

    private void UpdateMovement()
    {
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
                    _speedChange = t * Time.deltaTime;
                }
                else
                {
                    _speedChange = a * Time.deltaTime;
                }
            }
            else
            {
                _speedChange = da * Time.deltaTime;
            }

            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _speedChange);

            _rb.velocity = _velocity;
        }
    }

    private void InputCheck()
    {
        _isOnGround = _playerGround.isOnGround;

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

        _desiredVelocity = new Vector2(_directionX, 0f) * (_isOnGround ? maxSpeed : airMaxSpeed);
    }
}
