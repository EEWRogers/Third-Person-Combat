//     void Update() 
//     {

//         Debug.Log (attackAction.triggered);
//         Debug.Log (blockAction.ReadValue<float>());
//     }

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerSpeed = 2.0f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float jumpHeight = 1.0f;
    [SerializeField] float gravityValue = -9.81f;
    
    CharacterController controller;
    Vector3 playerVelocity;
    bool groundedPlayer;
    Transform cameraTransform;
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    InputAction blockAction;

    Weapon currentWeapon;
    Transform cameraTransformReference;
    

    void Awake() 
    {
        cameraTransformReference = new GameObject().transform; //creates a new game object to use as a reference
        currentWeapon = FindObjectOfType<Weapon>();
        currentWeapon.GetComponent<BoxCollider>().enabled = false;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
        blockAction = playerInput.actions["Block"];
    }

    void Update()
    {
        CheckIfGrounded();

        MovePlayer();

        JumpPlayer();

        RotateTowardsCamera();

        Attack();
    }

    void CheckIfGrounded()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }

    void MovePlayer()
    {
        cameraTransformReference.eulerAngles = new Vector3(0, cameraTransform.eulerAngles.y, 0); //allows us to access the camera's direction without accounting for rotation

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        // ties the movement direction to the camera direction
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransformReference.forward.normalized;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);
    }

    void JumpPlayer()
    {
        // Changes the height position of the player.
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void RotateTowardsCamera()
    {
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Attack()
    {
        if (attackAction.triggered)
        {
            GetComponent<Animator>().SetTrigger("attack");
        }
    }

    void EnableWeapon()
    {
        currentWeapon.GetComponent<BoxCollider>().enabled = true;
    }

        void DisableWeapon()
    {
        currentWeapon.GetComponent<BoxCollider>().enabled = false;
    }
}