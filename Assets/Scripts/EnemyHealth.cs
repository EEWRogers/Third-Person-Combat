using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float knockBackMultiplier = 100f;
    private float currentHealth;
    private Vector3 knockbackDirection;

    void Awake()
    {
        currentHealth = maxHealth;
        knockbackDirection = new Vector3();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Ouch! My health is now " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void KnockBack(float damageAmount, Vector3 playerPosition)
    {
        knockbackDirection = gameObject.transform.position - playerPosition;
        knockbackDirection.z = 0;
        knockbackDirection = knockbackDirection.normalized;

        gameObject.transform.position += knockbackDirection * Time.deltaTime * (damageAmount * knockBackMultiplier);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
