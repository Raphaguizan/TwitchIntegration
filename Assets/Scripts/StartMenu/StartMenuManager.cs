using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartMenuManager : MonoBehaviour
{
    public int maxPlayer = 6;
    public GameObject playerNamePrefab;
    public Transform content;
    public Camera mainCamera;
    public PlayersManager playerManager;

    private List<GameObject> playerController;
    private bool _canAddPlayer = true;

    private void OnEnable()
    {
        playerController = new List<GameObject>();
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
        if (playerController.Count >= maxPlayer)
        {
            _canAddPlayer = false;
            return;
        }
        if (Duplicate(data.Author)) return;

        var nameAux = Instantiate(playerNamePrefab, content);
        nameAux.GetComponentInChildren<TextMeshProUGUI>().text = data.Author;
        playerController.Add(nameAux);
        playerManager.AddPlayer(data);
    }
    public void AddPlayer(string name)
    {
        TwitchCommandData data = new TwitchCommandData { Author = name, Message = "" };
        AddPlayer(data);
    }

    private bool Duplicate(string playerName)
    {
        for (int i = 0; i < playerController.Count; i++)
        {
            var auxPlayer = playerController[i].GetComponentInChildren<TextMeshProUGUI>();
            if (auxPlayer.text.Equals(playerName))
            {
                return true;
            }
        }
        return false;
    }

    public void StartGame()
    {
        if (playerController.Count == 0) return;

        mainCamera.depth = -10;
        _canAddPlayer = false;
        gameObject.SetActive(false);
    }

    private void CleanList()
    {
        for (int i = 0; i < playerController.Count; i++)
        {
            Destroy(playerController[i]);
        }
        playerController.Clear();
    }
}
