using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 1f;

    private float currentHealth;

    private DamageFlash damageFlash;

    private void Start()
    {
        currentHealth = maxHealth;

        damageFlash = GetComponent<DamageFlash>();
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        damageFlash.CallDamageFlash();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
