using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int health;
    public List<InventoryItem> inventory;

    public PlayerData(Player player)
    {
        health = player.health; // temporary stuff
    }
}