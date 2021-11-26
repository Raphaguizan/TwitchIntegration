using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Twitch.Chat;
using Game.Util;
using System;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    public int maxPlayer = 36;
    public GameObject playerNamePrefab;
    public SO_Int secondsToStart;
    [Space]
    public Transform content;
    [SerializeField]
    private Scrollbar scrollBar;
    public TextMeshProUGUI numberOfPlayers;
    public TextMeshProUGUI StartButtonText;
    [Space]
    public Camera mainCamera;

    public static Action StartGameEvent;

    protected List<GameObject> _playerController;
    private bool _canAddPlayer = true;

    private void OnEnable()
    {
        _playerController = new List<GameObject>();
        mainCamera.depth = 10;
        _canAddPlayer = true;
        numberOfPlayers.text = "0";
        StartButtonText.text = "Começar";
    }
    private void OnDisable()
    {
        CleanList();
    }

    public void AddPlayer(TwitchCommandData data)
    {
        if (!_canAddPlayer) return;
        if (_playerController.Count >= maxPlayer)
        {
            _canAddPlayer = false;
            return;
        }
        if (Duplicate(data.Author)) return;

        var nameAux = Instantiate(playerNamePrefab, content);
        nameAux.GetComponentInChildren<TextMeshProUGUI>().text = data.Author;
        nameAux.GetComponentInChildren<TextMeshProUGUI>().color = data.Color;

        _playerController.Add(nameAux);
        RoundManager.AddPlayer(data);
        
        numberOfPlayers.text = _playerController.Count.ToString();

        if (_playerController.Count == 2) StartCoroutine(AutoStartTimer());
        StartCoroutine(ScrollBarWait());
    }
    IEnumerator ScrollBarWait()
    {
        yield return new WaitForEndOfFrame();
        scrollBar.value = 0;
    }

    protected IEnumerator AutoStartTimer()
    {
        int secondsCount = secondsToStart.value;
        int initialSecondCount = secondsCount;
        while (secondsCount >= 0)
        {
            StartButtonText.text = "começar... " + secondsCount.ToString("00");
            yield return new WaitForSecondsRealtime(1f);

            // envia mensagens de contagem para o chat
            if(secondsCount == 0)
            {
                TwitchChat.SendChatMessage("Em suas marcas...");
                TwitchChat.SendChatMessage("Preparar...");
                TwitchChat.SendChatMessage("dedsnaJii !");
            }
            else if(secondsCount % 10 == 0 || secondsCount == 5)
            {
                TwitchChat.SendChatMessage("Faltam "+secondsCount+" segundos para começar!");
            }

            // verifica se foi alterado o tempo e recomeça a contagem
            if(secondsToStart.value != initialSecondCount)
            {
                initialSecondCount = secondsToStart.value;
                secondsCount = initialSecondCount;
            }
            else
            {
                secondsCount--;
            }
        }
        StartGame();
    }

    public void AddPlayer(string name)
    {
        TwitchCommandData data = new TwitchCommandData { Author = name, Message = "!JII", Color = UnityEngine.Random.ColorHSV(0,1,1,1,0,1,1,1) };
        AddPlayer(data);
    }

    protected bool Duplicate(string playerName)
    {
        for (int i = 0; i < _playerController.Count; i++)
        {
            var auxPlayer = _playerController[i].GetComponentInChildren<TextMeshProUGUI>();
            if (auxPlayer.text.Equals(playerName))
            {
                return true;
            }
        }
        return false;
    }

    public virtual void StartGame()
    {
        if (_playerController.Count == 0) return;

        mainCamera.depth = -10;
        _canAddPlayer = false;
        gameObject.SetActive(false);
        StopAllCoroutines();
        StartGameEvent?.Invoke();
    }

    protected void CleanList()
    {
        for (int i = 0; i < _playerController.Count; i++)
        {
            Destroy(_playerController[i]);
        }
        _playerController.Clear();
    }
}
