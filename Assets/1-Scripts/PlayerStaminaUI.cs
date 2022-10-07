using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaUI : Singleton<PlayerStaminaUI>
{
    public Slider sliderRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetSliderValue(float value)
    {
        sliderRef.value = value;
    }
}
