using System.Collections;
using System.Collections.Generic;
using LDtkUnity;
using UnityEngine;

public class TeleportDoor : MonoBehaviour, ILocker
{
    public TeleportDoor toDoor;
    public Teleporter teleporter;
    public GameObject lockedVisual;

    public bool Lock { get; set; }

    private void Start()
    {
        var fields = GetComponent<LDtkFields>();
        toDoor = fields.GetEntityReference("teleport_to").FindEntity().GetComponent<TeleportDoor>();
        Lock = fields.GetBool("is_locked");
    }
    private void Update()
    {
        teleporter.to = toDoor.teleporter;
        teleporter.GetComponent<Collider2D>().enabled = !Lock && !toDoor.Lock;
        lockedVisual.SetActive(Lock);
    }


    private void OnDrawGizmos()
    {
        if (toDoor && !Lock)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, toDoor.transform.position + Vector3.up * 0.5f);
        }

    }
}
