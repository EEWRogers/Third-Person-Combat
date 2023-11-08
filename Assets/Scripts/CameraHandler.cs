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
    
    void Awake() 
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
        player = FindObjectOfType<PlayerInput>();
    }

    void Start() 
    {
        targetLockAction = player.actions["Target Lock"];
        cameraTransform = Camera.main.transform;
        EnableFollowCamera();
    }

    void Update() 
    {
        HandleRotation();
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

    public void HandleRotation()
    {
        if (cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject == followCamera.gameObject)
        {
            RotateTowardsCameraForward();
        }

        if (cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject == lockOnCamera.gameObject)
        {
            RotateTowardsTarget();
        }
    }

    void RotateTowardsCameraForward()
    {
        float cameraForward = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, cameraForward, 0);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = (closestEnemyByAngle.transform.position - player.transform.position);
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // locks camera behind player
        lockOnCamera.m_XAxis.Value = Mathf.LerpAngle(lockOnCamera.m_XAxis.Value, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
    }
}