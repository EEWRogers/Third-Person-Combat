using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] float weaponDamage = 1f;
    [SerializeField] float knockBackMultiplier = 100f;

    public BoxCollider weaponCollider;
    EnemyHealth targetHealth;
    Rigidbody targetRigidbody;
    Vector3 knockbackDirection;

    void Awake() 
    {
        weaponCollider = GetComponent<BoxCollider>();
        weaponCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        targetHealth = other.gameObject.GetComponent<EnemyHealth>();
        targetRigidbody = other.gameObject.GetComponent<Rigidbody>();

        if (targetHealth && targetRigidbody != null)
        {
            targetHealth.TakeDamage(weaponDamage);
            KnockBack(other.transform, targetRigidbody);
        }
    }

    void KnockBack(Transform enemyTransform, Rigidbody enemyRigidbody)
    {
        knockbackDirection = enemyTransform.position - gameObject.transform.position;
        knockbackDirection.z = 0;
        knockbackDirection = knockbackDirection.normalized;

        enemyRigidbody.AddForce(knockbackDirection * (weaponDamage * knockBackMultiplier));
    }

}
