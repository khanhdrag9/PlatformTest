using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    public float jumpHeight;
    public float timeToJumpApex;
    public float gravityMultiplier;


    bool _isOnGround;
    bool _desiredJump;
    bool _pressingJump;

    Rigidbody2D _rb;
    PlayerGround _playerGround;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerGround = GetComponent<PlayerGround>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        UpdatePhysics();
    }

    private void UpdatePhysics()
    {
        var newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        _rb.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravityMultiplier;
    }

    private void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _pressingJump = true;
            _desiredJump = true;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            _pressingJump = false;
        }
    }
}
