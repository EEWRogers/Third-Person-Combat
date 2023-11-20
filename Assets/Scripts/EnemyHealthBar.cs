using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] float highHealthValue = 0.7f;
    [SerializeField] float midHealthValue = 0.4f;
    [SerializeField] Color highHealthColour;
    [SerializeField] Color midHealthColour;
    [SerializeField] Color lowHealthColour;

    Slider slider;
    Camera mainCamera;
    Image healthBar;

    void Awake() 
    {
        mainCamera = FindObjectOfType<Camera>();
        slider = GetComponent<Slider>();
        healthBar = transform.GetChild (1).GetChild (0).gameObject.GetComponent<Image>();

        UpdateHealthBarColour();
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
        UpdateHealthBarColour();
    }

    void UpdateHealthBarColour()
    {
        if (slider.value >= highHealthValue)
        {
            healthBar.color = highHealthColour;
        }
        else if (slider.value < highHealthValue && slider.value > midHealthValue)
        {
            healthBar.color = midHealthColour;
        }
        else if (slider.value < midHealthValue)
        {
            healthBar.color = lowHealthColour;
        }

    }

}