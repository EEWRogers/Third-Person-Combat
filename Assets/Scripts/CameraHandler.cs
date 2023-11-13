using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook followCamera;
    [SerializeField] CinemachineFreeLook lockOnCamera;

    [SerializeField] float lockOnRange = 10f;
    [SerializeField] float maximumLockOnAngle = 90f;
    [SerializeField] float rotationSpeed = 5f;

    CinemachineBrain cinemachineBrain;
    PlayerInput player;
    InputAction targetLockAction;
    Transform cameraTransform;
    GameObject closestEnemyByAngle;
    float closestAngleToCamera;
    GameObject currentLockOnTarget;
    bool lockedOn = false;
    
    void Awake() 
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
        cameraTransform = Camera.main.transform;
        player = FindObjectOfType<PlayerInput>();
        targetLockAction = player.actions["Target Lock"];


        EnableFollowCamera();
    }

    void Update() 
    {
        HandleRotation();
        ScanForNearbyTargets();
        LockOnTarget();
        HandleMissingTarget();
    }

    public void HandleRotation()
    {
        if (cinemachineBrain.ActiveVirtualCamera == null) { return; }

        if (cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject == followCamera.gameObject)
        {
            RotateTowardsCameraForward();
        }

        if (cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject == lockOnCamera.gameObject)
        {
            if (currentLockOnTarget == null) { return; }
            RotateTowardsTarget();
        }
    }

    void ScanForNearbyTargets()
    {
        closestAngleToCamera = Mathf.Infinity;
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
            if (cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject == followCamera.gameObject && closestAngleToCamera <= maximumLockOnAngle)
            {
                EnableLockOnCamera();
            }

            else
            {
                EnableFollowCamera();
            }
        }
    }

    void HandleMissingTarget()
    {
        if (lockOnCamera.LookAt == null)
        {
            if (closestEnemyByAngle == null)
            {
                EnableFollowCamera();
            }
            
            else
            {
                if (lockedOn)
                {
                    EnableLockOnCamera();
                }
            }
        }
    }

    void EnableFollowCamera()
    {
        currentLockOnTarget = null;
        lockedOn = false;
        lockOnCamera.LookAt = null;
        followCamera.Priority = 10;
        lockOnCamera.Priority = 0;
    }

    void EnableLockOnCamera()
    {
        currentLockOnTarget = closestEnemyByAngle;
        lockedOn = true;
        lockOnCamera.LookAt = currentLockOnTarget.transform;
        followCamera.Priority = 0;
        lockOnCamera.Priority = 10;
    }

    void RotateTowardsCameraForward()
    {
        float cameraForward = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, cameraForward, 0);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = (currentLockOnTarget.transform.position - player.transform.position);
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        LockCameraBehindPlayer(targetRotation);
    }

    void LockCameraBehindPlayer(Quaternion targetRotation)
    {
        lockOnCamera.m_XAxis.Value = Mathf.LerpAngle(lockOnCamera.m_XAxis.Value, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
    }

}