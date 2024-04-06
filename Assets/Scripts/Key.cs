using System.Collections;
using System.Collections.Generic;
using LDtkUnity;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject locker;
    public float flySpeed;

    bool _triggered;

    private void Start()
    {
        var fields = GetComponent<LDtkFields>();
        locker = fields.GetEntityReference("locker").FindEntity().gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_triggered)
        {
            _triggered = true;
        }
    }

    private void Update()
    {
        if (_triggered)
        {
            transform.position = Vector2.MoveTowards(transform.position, locker.transform.position, flySpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, locker.transform.position) <= 0.1f)
            {
                locker.GetComponent<ILocker>().Lock = false;
                Destroy(gameObject);
            }
        }
    }
}
