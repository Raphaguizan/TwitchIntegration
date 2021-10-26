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

    public void Initialize(string name)
    {
        playerName = name;
        nameArea.text = name;
        nameArea.color = Random.ColorHSV();
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
