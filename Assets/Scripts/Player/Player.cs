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
    private GameObject _virtualCamera;
    [SerializeField]
    private TextMeshProUGUI _nameArea;
    
    [Space]
    [SerializeField]
    private EnergyBar _enegyBar;
    
    [Space]
    [SerializeField]
    private PlayerAnimations _animations;

    private void Start()
    {
        StartMenuManager.StartGameEvent += StartGameEvents;
    }

    public void Initialize(string name, Color color)
    {
        playerName = name;
        playerColor = color;
        _nameArea.text = name;
        _nameArea.color = playerColor;
        _enegyBar.SetFill(0);
        canCharge = false;
    }

    public void AddFill(float add, bool crit)
    {
        if (!canCharge) return;
        if (_enegyBar.currentFill >= 1)
        {
            // DO SOMETHING
            canCharge = false;
            return;
        }
        _enegyBar.AddFill(add);
        if(crit) _animations.TriggerCelebrate();
    }

    public Camera GetPlayerCamera()
    {
        return _playerCamera;
    }
    public void ChangeVirtualCameraLayer(int Layer)
    {
        _virtualCamera.layer = Layer;
    }

    private void StartGameEvents()
    {
        _animations.TriggerCharge();
        canCharge = true;
    }


    private void EndGameEvents(string name)
    {
        if (playerName.Equals(name))
        {
            _animations.TriggerWin();
        }
        else
        {
            _animations.TriggerLose();
        }
    }
}
