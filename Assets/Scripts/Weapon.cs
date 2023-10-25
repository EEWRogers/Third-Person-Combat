using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float weaponDamage = 1f;
    private EnemyHealth targetHealth;

private void OnTriggerEnter(Collider other)
{
    targetHealth = other.gameObject.GetComponent<EnemyHealth>();

    if (targetHealth != null)
    {
        targetHealth.TakeDamage(weaponDamage);
        targetHealth.KnockBack(weaponDamage, gameObject.transform.position);
    }
}
}
