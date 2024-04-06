using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterJump : MonoBehaviour
{
    public float jumpHeight;
    public float timeToJumpApex;
    public float jumpBuffer;
    public float coyoteTime;
    public float defaultGravityScale = 1;
    public float upwardMovementMultiplier;
    public float downwardMovementMultiplier;
    public float jumpCutOff;
    public float fallLimitSpeed;
    public float climbGravityScale;
    public UnityEvent onStartJump = new UnityEvent();
    public bool IsJumping => _currentlyJumping;


    bool _isOnGround;
    bool _desiredJump;
    bool _pressingJump;
    float _jumpBufferCounter;
    float _coyoteTimeCounter;
    bool _currentlyJumping;
    Vector2 _velocity;
    float _jumpSpeed;
    float _gravityMultiplier = 1;

    Rigidbody2D _rb;
    PlayerGround _playerGround;
    LadderMovement _ladderMovement;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerGround = GetComponent<PlayerGround>();
        _ladderMovement = GetComponent<LadderMovement>();
        _gravityMultiplier = defaultGravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        UpdatePhysics();
    }

    private void UpdatePhysics()
    {
        // var newGravity = new Vector2(0, -2 * jumpHeight / (timeToJumpApex * timeToJumpApex));
        // _rb.gravityScale = newGravity.y / Physics2D.gravity.y * _gravityMultiplier;

        _isOnGround = _playerGround.isOnGround;

        if (jumpBuffer > 0)
        {
            if (_desiredJump)
            {
                _jumpBufferCounter += Time.deltaTime;

                if (_jumpBufferCounter > jumpBuffer)
                {
                    _desiredJump = false;
                    _jumpBufferCounter = 0f;
                }
            }
        }

        if (!_currentlyJumping && !_isOnGround)
        {
            _coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            _coyoteTimeCounter = 0f;
        }
    }

    private void FixedUpdate()
    {
        _velocity = _rb.velocity;

        if (_desiredJump)
        {
            // Do a jump
            if (_isOnGround || _coyoteTimeCounter < coyoteTime && !_currentlyJumping)
            {
                _desiredJump = false;
                _jumpBufferCounter = 0;
                _coyoteTimeCounter = 0;

                _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _rb.gravityScale * jumpHeight);

                if (_velocity.y > 0f)
                {
                    _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
                }
                else if (_velocity.y < 0f)
                {
                    _jumpSpeed += Mathf.Abs(_rb.velocity.y);
                }

                _velocity.y += _jumpSpeed;
                _currentlyJumping = true;
                onStartJump.Invoke();
            }

            if (jumpBuffer == 0)
            {
                _desiredJump = false;
            }

            _rb.velocity = _velocity;
        }
        else
        {
            // Calculate gravity
            // Go up
            if (_rb.velocity.y > 0.01f)
            {
                if (_isOnGround)
                {
                    _gravityMultiplier = defaultGravityScale;
                }
                else
                {
                    if (_pressingJump && _currentlyJumping)
                    {
                        _gravityMultiplier = upwardMovementMultiplier;
                    }
                    else
                    {
                        _gravityMultiplier = jumpCutOff;
                    }
                }
            }
            // Go down
            else if (_rb.velocity.y < -0.01f)
            {
                if (_isOnGround)
                {
                    _gravityMultiplier = defaultGravityScale;
                }
                else
                {
                    _gravityMultiplier = downwardMovementMultiplier;
                }
            }
            // not vertically move
            else
            {
                if (_isOnGround)
                {
                    _currentlyJumping = false;
                }

                _gravityMultiplier = defaultGravityScale;
            }
        }

        if (_ladderMovement.isClimbing)
        {
            _rb.gravityScale = climbGravityScale;
        }
        else
        {
            var newGravity = new Vector2(0, -2 * jumpHeight / (timeToJumpApex * timeToJumpApex));
            _rb.gravityScale = newGravity.y / Physics2D.gravity.y * _gravityMultiplier;

            _rb.velocity = new Vector2(_velocity.x, Mathf.Clamp(_velocity.y, -fallLimitSpeed, 100));
        }

    }

    private void CheckInput()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (!_pressingJump)
            {
                _pressingJump = true;
                _desiredJump = true;
            }
        }
        else if (Input.GetAxis("Jump") == 0)
        {
            _pressingJump = false;
        }
    }
}
