using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject player;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerRigidbody = player.GetComponentInChildren<Rigidbody>();
    }

    public void PlacePlayer(Transform spawnPoint)
    {
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
        playerRigidbody.velocity = Vector3.zero;
    }
}
