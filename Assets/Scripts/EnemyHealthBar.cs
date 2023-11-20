using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] float highHealthValue = 0.7f;
    [SerializeField] float midHealthValue = 0.4f;
    [SerializeField] Color highHealthColour = new Color();
    [SerializeField] Color midHealthColour = new Color();
    [SerializeField] Color lowHealthColour = new Color();

    Slider slider;
    Camera mainCamera;

    void Awake() 
    {
        mainCamera = FindObjectOfType<Camera>();
        slider = GetComponent<Slider>();
    }

    void Update() 
    {
        if (mainCamera == null) { return; }
        transform.parent.rotation = mainCamera.transform.rotation;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (slider == null) { return; }
        slider.value = currentHealth / maxHealth;

    }

}