using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 10f;
    [SerializeField] float blockDamageReductionPercent = 50;

    float currentHealth;

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
        currentHealth -= damageAmount;
        Debug.Log("You only have " + currentHealth + " left!");

        if (currentHealth <= 0)
        {
            sceneLoader.ReloadLevel();
        }
    }

    void StartBlocking(InputAction.CallbackContext context)
    {
        Debug.Log("Blocking!");
    }

    void StopBlocking(InputAction.CallbackContext context)
    {
        Debug.Log("Stopped blocking!");
    }

}
