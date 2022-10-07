using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : Singleton<PlayerHealthUI>
{
    public Slider sliderRef;

    void Start()
    {
        
    }

    public void SetSliderValue(float value)
    {
        sliderRef.value = value;
    }
}
