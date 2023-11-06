using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook followCamera;
    [SerializeField] CinemachineFreeLook lockCamera;

    PlayerController playerController;

    void Awake() 
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void LockOnTarget()
    {
        if (playerController.TargetLocked && playerController.ClosestEnemyByAngle != null)
        {
            lockCamera.LookAt = playerController.ClosestEnemyByAngle.transform;
            Debug.Log("Locked to " + playerController.ClosestEnemyByAngle.gameObject.name);
            followCamera.gameObject.SetActive(false);
            lockCamera.gameObject.SetActive(true);
        }
        else if (!playerController.TargetLocked || playerController.ClosestEnemyByAngle == null)
        {
            followCamera.gameObject.SetActive(true);
            lockCamera.gameObject.SetActive(false);
        }
    }
}
