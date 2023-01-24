using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetSliderValueAsText : MonoBehaviour
{
    public bool linked;
    
    public string mainName, otherName;
    
    public Slider mainSlider, slider;

    public TMP_Text mainText, text;

    public void SetText()
    {
        mainText.text = mainName + ": " + mainSlider.value;
        
        if (linked)
        {
            slider.value = mainSlider.value;
            text.text = otherName + ": " + slider.value;
        }
    }

    public void SetLinked()
    {
        linked = !linked;
    }
}
