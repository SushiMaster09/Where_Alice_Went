using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private LevelConnection connection;
    [SerializeField] private string targetSceneName;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        if (connection == LevelConnection.ActiveConnection)
        {
            PlayerMovement.instance.transform.position = spawnPoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<PlayerMovement>();

        if (player != null)
        {
            LevelConnection.ActiveConnection = connection;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
