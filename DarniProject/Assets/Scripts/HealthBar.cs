using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Image highLight;
    public Image darkLight;

    void Start()
    {
        fill.color = gradient.Evaluate(slider.normalizedValue);

        Color.RGBToHSV(fill.color, out float h, out float s, out float v);

        float highlightH = (h + 0.03f) % 1f;  
        float highlightV = Mathf.Clamp01(v * 1.35f);

        if (highLight != null)
            highLight.color = Color.HSVToRGB(highlightH, s, highlightV);

        float shadowH = (h - 0.02f + 1f) % 1f; 
        float shadowV = Mathf.Clamp01(v * 0.6f);

        if (darkLight != null)
            darkLight.color = Color.HSVToRGB(shadowH, s, shadowV);
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        float v = slider.normalizedValue;

        fill.fillAmount = v;
        if (highLight) highLight.fillAmount = v;
        if (darkLight) darkLight.fillAmount = v;

    }
}
