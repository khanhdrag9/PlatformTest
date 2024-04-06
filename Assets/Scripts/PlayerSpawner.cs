using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    static GameObject player = null;

    void Start()
    {
        if(player)
        {
            return;
        }

        player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        FindFirstObjectByType<CinemachineCamera>().Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
