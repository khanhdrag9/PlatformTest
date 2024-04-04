using System;
using System.Collections;
using System.Collections.Generic;
using LDtkUnity;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPushAbility : MonoBehaviour
{
    public LayerMask pushLayerMask;
    public Vector3 offset;
    public float distance;
    public bool bothSide;
    public float moveSpeedScale = 0.5f;
    public bool releaseWhenJump;

    PlayerMovement _playerMovement;
    PlayerGround _playerGround;
    CharacterJump _characterJump;
    GameObject _currentItem;
    bool pressed;
    Rigidbody2D _rb;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerGround = GetComponent<PlayerGround>();
        _characterJump = GetComponent<CharacterJump>();
        _characterJump.onStartJump.AddListener(() =>
        {
            Release();
        });
        _rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        bool input = _playerGround.isOnGround && Input.GetAxis("Push") > 0;
        bool onPressed = false;
        if (input && !pressed)
        {
            pressed = true;
            onPressed = true;
        }

        if (!input)
        {
            pressed = false;
        }


        if (onPressed)
        {
            if (_currentItem)
            {
                Release();
            }
            else
            {
                var hit = CastRaycast();
                if (hit)
                {
                    var joint = hit.GetComponent<FixedJoint2D>();
                    joint.connectedBody = _rb;

                    _currentItem = hit;
                }
            }
        }

        if (_currentItem)
            _playerMovement.maxSpeedScale = moveSpeedScale;
        else
            _playerMovement.maxSpeedScale = 1;

    }

    GameObject CastRaycast()
    {
        var info = Physics2D.Raycast(transform.position + offset, Mathf.Sign(transform.localScale.x) * Vector2.right, distance, pushLayerMask);
        if (!info.collider)
        {
            if (bothSide)
            {
                info = Physics2D.Raycast(transform.position + offset, Mathf.Sign(transform.localScale.x) * Vector2.left, distance, pushLayerMask);
            }
        }

        return info.collider ? info.collider.gameObject : null;
    }

    public void Release()
    {
        if (_currentItem)
        {
            var joint = _currentItem.GetComponent<FixedJoint2D>();
            joint.connectedBody = null;
            _currentItem = null;
        }
    }
}
