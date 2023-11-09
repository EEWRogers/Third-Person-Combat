using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 10f;
    [SerializeField] float blockDamageReductionPercent = 50;
    [SerializeField] float invulnerabilityTime = 1f;

    float currentHealth;
    float damageCooldownTime = 0f;
    bool isBlocking = false;
    public bool IsBlocking { get { return isBlocking; } }

    SceneLoader sceneLoader;
    Animator playerAnimator;
    PlayerInput playerInput;
    InputAction blockAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        blockAction = playerInput.actions["Block"];

        sceneLoader = FindObjectOfType<SceneLoader>();
        playerAnimator = GetComponent<Animator>();
        
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

    void Update() 
    {
        if (damageCooldownTime > 0)
        {
            damageCooldownTime -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (damageCooldownTime > 0) { return; }

        damageCooldownTime = invulnerabilityTime;
        
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
        playerAnimator.SetBool("blocking", isBlocking);
    }

    void StopBlocking(InputAction.CallbackContext context)
    {
        isBlocking = false;
        playerAnimator.SetBool("blocking", isBlocking);
    }

}
