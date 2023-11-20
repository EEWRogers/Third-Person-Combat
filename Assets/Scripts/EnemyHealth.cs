using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 10f;
    float currentHealth;

     EnemyHealthBar healthbar;

    void Awake()
    {
        currentHealth = maxHealth;
        healthbar = GetComponentInChildren<EnemyHealthBar>();

        healthbar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        healthbar.UpdateHealthBar(currentHealth, maxHealth);
        
        Debug.Log("Ouch! My health is now " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
