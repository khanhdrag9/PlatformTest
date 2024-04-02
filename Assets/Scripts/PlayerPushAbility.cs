using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPushAbility : MonoBehaviour
{
    public GameObject pushRb;
    public LayerMask pushLayerMask;
    public Vector3 offset;
    public float distance;
    public bool bothSide;

    PlayerGround _playerGround;
    Transform _pushingItem;
    bool pressed;

    private void Start()
    {
        _playerGround = GetComponent<PlayerGround>();
    }

    private void Update()
    {
        bool input = _playerGround.isOnGround && Input.GetAxis("Push") > 0;
        if (input && !pressed)
        {
            pressed = true;
            if (_pushingItem)
            {
                _pushingItem?.SetParent(null);
                _pushingItem = null;
            }
            else
            {
                GetPushAround();
            }
        }

        if (!input)
        {
            pressed = false;
        }

        if(_pushingItem)
        {
            GetPushAround();
        }
    }

    Transform GetPushAround()
    {
        var info = Physics2D.Raycast(transform.position + offset, Mathf.Sign(transform.localScale.x) * Vector2.right, distance, pushLayerMask);
        if (!info.collider)
        {
            if (bothSide)
            {
                info = Physics2D.Raycast(transform.position + offset, Mathf.Sign(transform.localScale.x) * Vector2.left, distance, pushLayerMask);
            }
        }

        if (info.collider)
        {
            if (_pushingItem != info.collider.transform)
            {
                info.collider.transform.SetParent(pushRb.transform);
            }
            _pushingItem = info.collider.transform;
        }
        else
        {
            _pushingItem?.SetParent(null);
            _pushingItem = null;
        }

        return _pushingItem;
    }

    void SetPushingItem(Transform item)
    {
    }
}
