using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Credit to https://www.youtube.com/watch?v=BLfNP4Sc_iA

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentHealth(float health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
}
