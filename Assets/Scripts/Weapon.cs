using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float weaponDamage = 1f;
    [SerializeField] private float knockBackMultiplier = 100f;
    private EnemyHealth targetHealth;
    private Rigidbody targetRigidbody;
    private Transform player;
    private Vector3 knockbackDirection;

    void Awake()
    {
        player = gameObject.GetComponentInParent<Transform>();
    }

    void OnTriggerEnter(Collider other)
    {
        targetHealth = other.gameObject.GetComponent<EnemyHealth>();
        targetRigidbody = other.gameObject.GetComponent<Rigidbody>();

        if (targetHealth && targetRigidbody != null)
        {
        targetHealth.TakeDamage(weaponDamage);
        KnockBack(other.transform, other.GetComponent<Rigidbody>());
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
