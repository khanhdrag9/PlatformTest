using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGround : MonoBehaviour
{
    public bool isOnGround;
    public float groundLength;
    public Vector2 colliderOffset;
    public LayerMask groundLayer;

    private void Update()
    {
        isOnGround = Physics2D.Raycast(transform.position + (Vector3)colliderOffset, Vector2.down, groundLength, groundLayer);
    }
}
