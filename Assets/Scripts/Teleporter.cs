using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Teleporter to;
    public List<GameObject> ignores = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(ignores.Contains(other.gameObject))
            return;

        Trigger(other.gameObject);
        ignores.Add(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ignores.Remove(other.gameObject);
    }

    public void Trigger(GameObject target)
    {
        target.transform.position = target.transform.position - transform.position + to.transform.position;
        to.ignores.Add(target);

        target.GetComponentInChildren<TrailRenderer>().Clear();
    }

}
