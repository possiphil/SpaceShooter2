using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    public void SetMaxProgress(float progress)
    {
        slider.maxValue = progress;
        slider.value = progress;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetProgress(float progress)
    {
        slider.value = progress;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    } 
}
