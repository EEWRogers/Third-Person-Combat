using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 10f;
    float currentHealth;
    SceneLoader sceneLoader;

    void Awake()
    {
        currentHealth = maxHealth;
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("You only have " + currentHealth + " left!");

        if (currentHealth <= 0)
        {
            sceneLoader.ReloadLevel();
        }
    }

}
