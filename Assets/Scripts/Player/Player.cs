using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;

    public int health;

    private DamageFlash damageFlash;

    private void Start()
    {
        health = maxHealth;
        damageFlash = GetComponent<DamageFlash>();
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPLayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        health = data.health;
    }

    public void DamagePlayer(int amount)
    {
        health -= amount;
        damageFlash.CallDamageFlash();
    }
}
