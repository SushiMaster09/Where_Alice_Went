using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Sprite[] healthSprites;

    private Image image;
    private Player player;

    private void Awake()
    {
        image = GetComponent<Image>();
        player = FindAnyObjectByType<Player>();
    }

    private void Update()
    {
        image.sprite = healthSprites[player.health - 1];
    }
}
