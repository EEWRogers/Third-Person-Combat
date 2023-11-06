using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] float weaponDamage = 1f;
    PlayerHealth playerHealth;
    Rigidbody targetRigidbody;
    Vector3 knockbackDirection;

    void OnTriggerEnter(Collider other)
    {
        playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        targetRigidbody = other.gameObject.GetComponent<Rigidbody>();

        if (playerHealth && targetRigidbody != null)
        {
            playerHealth.TakeDamage(weaponDamage);
        }
    }
}
