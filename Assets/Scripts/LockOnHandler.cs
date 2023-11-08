using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

public class LockOnHandler : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook followCamera;
    [SerializeField] CinemachineFreeLook lockOnCamera;

    [SerializeField] float lockOnRange = 10f;
    [SerializeField] float maximumLockOnAngle = 90f;

    CinemachineBrain cinemachineBrain;

    PlayerInput player;
    InputAction targetLockAction;
    GameObject closestEnemyByAngle;
    
    void Awake() 
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
        player = FindObjectOfType<PlayerInput>();
    }

    void Start() 
    {
        targetLockAction = player.actions["Target Lock"];
        EnableFollowCamera();
    }

    void Update() 
    {
        ScanForNearbyTargets();
        LockOnTarget();
        Debug.Log(targetLockAction.triggered);
        Debug.Log(closestEnemyByAngle);
    }

    void ScanForNearbyTargets()
    {
        float closestAngleToCamera = Mathf.Infinity;
        Collider [] nearbyTargets = Physics.OverlapSphere(player.transform.position, lockOnRange);
        foreach (Collider target in nearbyTargets)
        {

            if (target.CompareTag("Enemy"))
            {
                Vector3 targetDirection = target.transform.position - player.transform.position;
                float targetAngleToCamera = Vector3.Angle(player.transform.forward, targetDirection);
                if (targetAngleToCamera < closestAngleToCamera)
                {
                    closestAngleToCamera = targetAngleToCamera;
                    closestEnemyByAngle = target.gameObject;
                }
            }

        }
    }

    void LockOnTarget()
    {
        if (targetLockAction.triggered)
        {
            if (cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject == followCamera.gameObject)
            {
                EnableLockOnCamera();
            }

            else
            {
                EnableFollowCamera();
            }
        }
    }

    void EnableFollowCamera()
    {
        lockOnCamera.LookAt = null;
        followCamera.Priority = 10;
        lockOnCamera.Priority = 0;
    }

    void EnableLockOnCamera()
    {
        lockOnCamera.LookAt = closestEnemyByAngle.transform;
        followCamera.Priority = 0;
        lockOnCamera.Priority = 10;
    }

    void RotateTowardsTarget()
    {
        float cameraForward = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, cameraForward, 0);
        transform.rotation = Quaternion.Lerp(transform. rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
