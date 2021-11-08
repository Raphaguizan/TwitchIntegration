using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public string playerName;
    public Color playerColor;
    public float runningSpeed;
    public float fallTime;
    public bool CanCharge { get; private set; }

    [SerializeField]
    private Camera _playerCamera;
    [SerializeField]
    private GameObject _virtualCamera;
    [SerializeField]
    private TextMeshProUGUI _nameArea;
    
    [Space]
    [SerializeField]
    private EnergyBar enegyBar;
    
    [Space]
    [SerializeField]
    private PlayerAnimations animations;

    private bool _isRunning;
    private Vector3 finishLine;

    private void Start()
    {
        StartMenuManager.StartGameEvent += StartGameEvents;
        PlayersManager.vitoriousPlayerEvent += name => EndGameEvents(name);
    }

    public void Initialize(string name, Color color, Vector3 finish)
    {
        playerName = name;
        playerColor = color;
        _nameArea.text = name;
        finishLine = finish;
        _nameArea.color = playerColor;
        enegyBar.SetFill(0);

        _isRunning = false;
        CanCharge = false;
    }

    public Camera GetPlayerCamera()
    {
        return _playerCamera;
    }
    public void ChangeVirtualCameraLayer(int Layer)
    {
        _virtualCamera.layer = Layer;
    }

    #region fill
    public void AddFill(float add, bool crit)
    {
        if (!CanCharge) return;
        if (enegyBar.currentFill >= 1)
        {
            StartRun();
            return;
        }
        enegyBar.AddFill(add);
        if(crit) animations.TriggerCelebrate();
    }
    #endregion

    #region run
    private void Update()
    {
        if (_isRunning && PlayersManager.IsPlaying)
        {
            transform.Translate(new Vector3(0,0, runningSpeed * Time.deltaTime));
            if(transform.position.z > finishLine.z)
            {
                PlayersManager.Instance.ChampionPlayer(playerName);
            }
        }
    }

    public void StartRun()
    {
        CanCharge = false;
        _isRunning = true;
        animations.TriggerRun(_isRunning);
        StartCoroutine(FallRandom());
    }

    IEnumerator FallRandom()
    {
        while (PlayersManager.IsPlaying)
        {
            yield return new WaitForSeconds(5f);
            if (Random.value > enegyBar.currentFill)
            {
                Debug.Log("caiu!!!");
                if(_isRunning)StartCoroutine(FallWait());
            }
        }
    }

    IEnumerator FallWait()
    {
        _isRunning = false;
        animations.TriggerRun(_isRunning);
        animations.TriggerFall();
        yield return new WaitForSeconds(fallTime);
        _isRunning = true;
        animations.TriggerRun(_isRunning);
    }
    #endregion

    #region game Events
    private void StartGameEvents()
    {
        animations.TriggerCharge();
        CanCharge = true;
    }

    private void EndGameEvents(string name)
    {
        if (playerName.Equals(name))
        {
            animations.TriggerWin();
        }
        else
        {
            animations.TriggerLose();
        }
    }
    #endregion
}
