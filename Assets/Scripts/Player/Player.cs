using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public string playerName;
    public Color playerColor;
    public bool canCharge { get; private set; }

    [SerializeField]
    private Camera _playerCamera;
    [SerializeField]
    private GameObject virtualCamera;
    [SerializeField]
    private TextMeshProUGUI nameArea;
    
    [Space]
    [SerializeField]
    private EnergyBar enegyBar;

    public void Initialize(string name, Color color)
    {
        playerName = name;
        playerColor = color;
        nameArea.text = name;
        nameArea.color = playerColor;
        enegyBar.SetFill(0);
        canCharge = true;
    }

    public void AddFill(float add)
    {
        if (!canCharge) return;
        if (enegyBar.currentFill >= 1)
        {
            // DO SOMETHING
            canCharge = false;
            return;
        }
        enegyBar.AddFill(add);
    }

    public Camera GetPlayerCamera()
    {
        return _playerCamera;
    }
    public void ChangeVirtualCameraLayer(int Layer)
    {
        virtualCamera.layer = Layer;
    }
}
