using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    public TeleportDoor toDoor;
    public Teleporter teleporter;

    private void Start()
    {
    }
    private void Update()
    {
        teleporter.to = toDoor.teleporter;
    }


    private void OnDrawGizmos()
    {
        if (toDoor)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, toDoor.transform.position + Vector3.up * 0.5f);
        }

    }
}
