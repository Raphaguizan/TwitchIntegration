using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Image fill;

    public float currentFill { get; private set; }

    public void SetFill(float f)
    {
        currentFill = f;
        fill.fillAmount = currentFill;
        fill.color = FillColorAdjust();
    }

    public void AddFill(float f)
    {
        currentFill += f;
        SetFill(currentFill);
    }

    private Color FillColorAdjust()
    {
        float f = fill.fillAmount;
        if (f < .2f)
        {
            return Color.red;
        }
        else if (f < .5f)
        {
            return Color.yellow;
        }
        else
        {
            return Color.green;
        }
    }
    private void OnValidate()
    {
        Debug.Log("validate");
        fill.color = FillColorAdjust();
    }

}
