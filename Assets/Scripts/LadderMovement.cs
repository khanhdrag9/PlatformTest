using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    public float climbSpeed;
    public LayerMask layerMask;
    public bool isClimbing;

    BoxCollider2D _collider;
    Rigidbody2D _rb;
    float _directionY;
    PlayerGround _playerGround;
    CharacterJump _characterJump;


    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _playerGround = GetComponent<PlayerGround>();
        _characterJump = GetComponent<CharacterJump>();
        _characterJump.onStartJump.AddListener(() =>
        {
            isClimbing = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(!_playerGround.isOnGround && !isClimbing)
        {
            var below = Physics2D.Raycast(transform.position + (Vector3)_playerGround.colliderOffset, Vector2.down, _playerGround.groundLength, layerMask);
            if(below.collider && below.collider.CompareTag("Ladder"))
            {
                isClimbing = true;
            }
        }

        var hit = Physics2D.OverlapBox(transform.position + (Vector3)_collider.offset, _collider.size, 0, layerMask);
        _directionY = Input.GetAxis("Vertical");

        if (hit && hit.CompareTag("Ladder") && (_directionY != 0 || isClimbing))
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }


    }

    private void FixedUpdate()
    {
        if(isClimbing)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _directionY * climbSpeed);
        }
    }
}
