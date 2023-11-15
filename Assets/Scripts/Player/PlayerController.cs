using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput), typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerSpeed = 2.0f;
    [SerializeField] float movementAccelerationDelay = 0.2f;
    [SerializeField] float jumpHeight = 1.0f;
    [SerializeField] float gravityValue = -9.81f;
    [SerializeField] float dodgeStrength = 2.0f;
    [SerializeField] float dodgeLength = 1.0f;
    [SerializeField] float attackTimeout = 1.0f;
    
    CharacterController controller;
    Animator playerAnimator;
    PlayerHealth playerHealth;
    PlayerInput playerInput;
    PlayerWeapon playerWeapon;

    Vector3 playerVelocity;
    Vector2 currentInputVector;
    Vector2 smoothInputVelocity;
    bool playerGrounded;
    bool isDodging = false;
    
    int currentAttack = 0;
    int attackQueue = 0;

    Transform cameraTransform;
    Transform cameraTransformReference;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    InputAction dodgeAction;


    void Awake() 
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
        dodgeAction = playerInput.actions["Dodge"];

        controller = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAnimator = GetComponent<Animator>();

        cameraTransform = Camera.main.transform;
        cameraTransformReference = new GameObject().transform; //creates a new game object to use as a reference

        playerWeapon = GetComponentInChildren<PlayerWeapon>();
    }

    void OnEnable() 
    {
        attackAction.performed += Attack;
        dodgeAction.performed += Dodge;
    }

    void OnDisable() 
    {
        attackAction.performed -= Attack;
        dodgeAction.performed -= Dodge;
    }

    void Update()
    {
        CheckIfGrounded();

        MovePlayer();
        
        JumpPlayer();
    }

    void CheckIfGrounded()
    {
        playerGrounded = controller.isGrounded;
        if (playerGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }

    void MovePlayer()
    {
        cameraTransformReference.eulerAngles = new Vector3(0, cameraTransform.eulerAngles.y, 0); //allows us to access the camera's direction without accounting for rotation

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, movementAccelerationDelay);
        Vector3 move = new Vector3(currentInputVector.x, 0, currentInputVector.y);

        // ties the movement direction to the camera direction
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransformReference.forward.normalized;
        move.y = 0f;

        if (!isDodging)
        {
            controller.Move(move * Time.deltaTime * playerSpeed);
        }
        
        else
        {
            controller.Move(move * Time.deltaTime * dodgeStrength);
        }
    }

    void JumpPlayer()
    {
        // Changes the height position of the player.
        if (jumpAction.triggered && playerGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Attack(InputAction.CallbackContext context)
    {
        if (playerHealth.IsBlocking) { return; }

        attackQueue++;

        if (attackQueue > 3)
        {
            attackQueue = 3;
        }

        StartCoroutine(PerformAttack());

    }

    IEnumerator PerformAttack()
    {
        currentAttack = attackQueue;

        SetAttackAnimation(currentAttack);

        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        yield return new WaitForSeconds(attackTimeout);

        if (currentAttack == attackQueue)
        {
            SetAttackAnimation(0);
            attackQueue = 0;
        }

        if (currentAttack < attackQueue)
        {
            StartCoroutine(PerformAttack());
        }
    }

    void SetAttackAnimation(int attackNumber)
    {
        playerAnimator.SetInteger("attack", attackNumber);
    }

    void Dodge(InputAction.CallbackContext context)
    {
        if (!isDodging && !playerHealth.IsBlocking)
        {
            StartCoroutine(PerformDodge());
        }
    }

    IEnumerator PerformDodge()
    {
        isDodging = true;
        playerHealth.canBlock = false;
        Debug.Log("Dodging!");

        yield return new WaitForSeconds(dodgeLength);

        isDodging = false;
        playerHealth.canBlock = true;
    }

    void EnableWeapon()
    {
        playerWeapon.weaponCollider.enabled = true;
    }

    void DisableWeapon()
    {
        playerWeapon.weaponCollider.enabled = false;
    }

}