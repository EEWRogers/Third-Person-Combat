using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Camera mainCamera;

    void Awake() 
    {
        slider = GetComponent<Slider>();
    }

    void Update() 
    {
        transform.parent.rotation = mainCamera.transform.rotation;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
    }

}
