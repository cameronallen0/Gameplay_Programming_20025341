using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public int minHealth = 0;
    public int maxHealth = 100;
    
    public void SetMaxHealth(int health)
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.minValue = minHealth;
        slider.value = health;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        if(health == minHealth)
        {
            slider.value = maxHealth;
        }
    }
}
