using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform respawnPoint;

    [SerializeField] private Player player;

    private void Update()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.DamagePlayer(damage);
            player.transform.position = respawnPoint.position;
        }
    }
}
