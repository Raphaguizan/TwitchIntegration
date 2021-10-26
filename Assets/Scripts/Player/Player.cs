using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public string playerName;

    [SerializeField]
    private Camera _playerCamera;
    [SerializeField]
    private GameObject virtualCamera;
    [SerializeField]
    private TextMeshProUGUI nameArea;
    
    [Space]
    [SerializeField]
    private EnergyBar enegyBar;

    public void Initialize(string name)
    {
        playerName = name;
        nameArea.text = name;
        nameArea.color = Random.ColorHSV();
        enegyBar.SetFill(0);
    }

    public void AddFill(float add)
    {
        if (enegyBar.currentFill >= 1)
        {
            // DO SOMETHING
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
