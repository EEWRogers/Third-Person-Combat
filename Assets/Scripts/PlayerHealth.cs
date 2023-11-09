using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 10f;
    [SerializeField] float blockDamageReductionPercent = 50;

    float currentHealth;
    bool isBlocking = false;

    SceneLoader sceneLoader;
    PlayerInput playerInput;
    InputAction blockAction;

    void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        playerInput = GetComponent<PlayerInput>();
        blockAction = playerInput.actions["Block"];
        currentHealth = maxHealth;
    }

    void OnEnable() 
    {
        blockAction.started += StartBlocking;
        blockAction.canceled += StopBlocking;
    }

    void OnDisable() 
    {
        blockAction.started -= StartBlocking;
        blockAction.canceled -= StopBlocking;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isBlocking)
        {
            currentHealth -= (damageAmount * (blockDamageReductionPercent / 100));
        }
        
        else
        {
            currentHealth -= damageAmount;
        }
        
        Debug.Log("You only have " + currentHealth + " left!");

        if (currentHealth <= 0)
        {
            sceneLoader.ReloadLevel();
        }
    }

    void StartBlocking(InputAction.CallbackContext context)
    {
        isBlocking = true;
        Debug.Log(isBlocking);
    }

    void StopBlocking(InputAction.CallbackContext context)
    {
        isBlocking = false;
        Debug.Log(isBlocking);
    }

}
