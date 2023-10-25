using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float weaponDamage = 1f;
    private EnemyHealth targetHealth;
    private Transform player;

    void Awake()
    {
        player = gameObject.GetComponentInParent<Transform>();
    }

    void OnTriggerEnter(Collider other)
    {
    targetHealth = other.gameObject.GetComponent<EnemyHealth>();

        if (targetHealth != null)
        {
        targetHealth.TakeDamage(weaponDamage);
        targetHealth.KnockBack(weaponDamage, player.position);
        }
    }

}
