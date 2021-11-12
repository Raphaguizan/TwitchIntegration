using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Twitch.Chat;
using System;

public class StartMenuManager : MonoBehaviour
{
    public int maxPlayer = 36;
    public GameObject playerNamePrefab;
    public Transform content;
    public Camera mainCamera;
    public RoundManager roundManager;

    public static Action StartGameEvent;

    private List<GameObject> _playerController;
    private bool _canAddPlayer = true;

    private void OnEnable()
    {
        _playerController = new List<GameObject>();
        mainCamera.depth = 10;
        _canAddPlayer = true;
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
        roundManager.AddPlayer(data);
    }
    public void AddPlayer(string name)
    {
        TwitchCommandData data = new TwitchCommandData { Author = name, Message = "!JII", Color = UnityEngine.Random.ColorHSV(0,1,1,1,0,1,1,1) };
        AddPlayer(data);
    }

    private bool Duplicate(string playerName)
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

    public void StartGame()
    {
        if (_playerController.Count == 0) return;

        mainCamera.depth = -10;
        _canAddPlayer = false;
        gameObject.SetActive(false);
        StartGameEvent?.Invoke();
    }

    private void CleanList()
    {
        for (int i = 0; i < _playerController.Count; i++)
        {
            Destroy(_playerController[i]);
        }
        _playerController.Clear();
    }
}
