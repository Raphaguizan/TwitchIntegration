using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Image fill;

    public void SetFill(float f)
    {
        fill.fillAmount = f;
        fill.color = FillColorAdjust();
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
