using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook followCamera;
    [SerializeField] CinemachineFreeLook lockOnCamera;

    CinemachineBrain cinemachineBrain;
    PlayerController playerController;

    void Awake() 
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
        playerController = FindObjectOfType<PlayerController>();
        EnableFollowCamera();
    }

    public void LockOnTarget()
    {
        if (playerController.TargetLocked && playerController.ClosestEnemyByAngle != null)
        {
            if (cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject == followCamera.gameObject)
            {
                EnableLockOnCamera();
            }
            // lockOnCamera.LookAt = playerController.ClosestEnemyByAngle.transform;
            // Debug.Log("Locked to " + playerController.ClosestEnemyByAngle.gameObject.name);
        }
        else if (!playerController.TargetLocked || playerController.ClosestEnemyByAngle == null)
        {
            EnableFollowCamera();
        }
    }

    void EnableFollowCamera()
    {
        followCamera.Priority = 10;
        lockOnCamera.Priority = 0;
    }

    void EnableLockOnCamera()
    {
        followCamera.Priority = 0;
        lockOnCamera.Priority = 10;
    }
}
