using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamage;

    [SerializeField]private int health = 100;
    private int maxHealth;

    private void Awake()
    {
        maxHealth = 100;
    }

    public void DealDamage(int damageAmount)
    {
        health -= damageAmount;

        OnDamage?.Invoke(this, EventArgs.Empty);

        if(health <= 0)
        {
            health = 0;
            Die();
        }

    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / maxHealth;
    }
}
