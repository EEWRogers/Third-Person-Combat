using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] float weaponDamage = 1f;

    public BoxCollider weaponCollider;
    PlayerHealth playerHealth;
    Rigidbody targetRigidbody;

    void Awake() 
    {
        weaponCollider = GetComponent<BoxCollider>();
        weaponCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        playerHealth = other.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(weaponDamage);
        }
    }
    
}
