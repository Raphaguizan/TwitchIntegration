using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NextRoundMenu : StartMenuManager
{
    private int numOfPlayers;
    [Space]
    [SerializeField]
    private float waitTimeToOpen = 5f;
    [SerializeField]
    private GameObject window;

    private void OnEnable()
    {
        PlayersManager.vitoriousPlayerEvent += name => StartCoroutine(WaitToOpen());
    }
    private void OnDisable()
    {
        PlayersManager.vitoriousPlayerEvent -= name => StartCoroutine(WaitToOpen());
    }
    private void EnableWindow()
    {
        if(_playerController == null) _playerController = new List<GameObject>();
        else CleanList();

        mainCamera.depth = 10;
        numOfPlayers = RoundManager.Instance.ListOfPlayerPerRound[RoundManager.Instance.currentRound];
        numberOfPlayers.text = numOfPlayers.ToString();
        for (int i = 0; i < numOfPlayers; i++)
        {
            var nameAux = Instantiate(playerNamePrefab, content);
            nameAux.GetComponentInChildren<TextMeshProUGUI>().text = RoundManager.Instance.listOfPlayers[i].Author;
            nameAux.GetComponentInChildren<TextMeshProUGUI>().color = RoundManager.Instance.listOfPlayers[i].Color;
            _playerController.Add(nameAux);
        }
        StartCoroutine(AutoStartTimer());
    }

    public override void StartGame()
    {
        if (_playerController.Count == 0) return;

        mainCamera.depth = -10;
        window.SetActive(false);
        RoundManager.StartRound();
    }

    IEnumerator WaitToOpen()
    {
        Debug.Log("entrou no wait");
        yield return new WaitForSeconds(waitTimeToOpen);
        yield return new WaitUntil(()=> RoundManager.PlayerIsBalanced);
        window.SetActive(true);
        EnableWindow();
    }
}
