using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;
using Twitch.Chat;

public class RoundManager : Singleton<RoundManager>
{
    [SerializeField]
    public readonly int playerPerRoundLimit = 6;
    //public int listOfPlayerCount_teste = 32;

    [SerializeField]
    private int numberOfRounds = 0;
    [SerializeField]
    public int currentRound = 0;

    [SerializeField]
    public List<TwitchCommandData> listOfPlayers;
    [SerializeField]
    public List<TwitchCommandData> listOfWinners;

    public List<int> ListOfPlayerPerRound { get; private set; }
    
    public static bool PlayerIsBalanced = false;

    private void OnEnable()
    {
        listOfPlayers = new List<TwitchCommandData>();
        listOfWinners = new List<TwitchCommandData>();
        ListOfPlayerPerRound = new List<int>();

        StartMenuManager.StartGameEvent += StartGame;
        PlayersManager.vitoriousPlayerEvent += name => EndRound(name);
    }

    private void OnDisable()
    {
        StartMenuManager.StartGameEvent -= StartGame;
        PlayersManager.vitoriousPlayerEvent -= name => EndRound(name);
    }

    public void AddPlayer(TwitchCommandData data)
    {
        listOfPlayers.Add(data);
    }

    public void BalancePlayers()
    {
        Debug.Log("aki");
        ListOfPlayerPerRound.Clear();
        if (listOfPlayers.Count % playerPerRoundLimit == 0)
        {
            numberOfRounds = listOfPlayers.Count / playerPerRoundLimit;
        }
        else
        {
            numberOfRounds = (int)Mathf.Ceil(((float)listOfPlayers.Count / (float)playerPerRoundLimit));
        }
        int playerPerRound = (int)Mathf.Floor((float)listOfPlayers.Count / (float)numberOfRounds);

        int restPlayers = listOfPlayers.Count - playerPerRound*numberOfRounds;
        for (int i = 0; i < numberOfRounds; i++)
        {
            ListOfPlayerPerRound.Add(playerPerRound);
            if(restPlayers > 0)
            {
                ListOfPlayerPerRound[i]++;
                restPlayers--;
            }
        }
        PlayerIsBalanced = true;
    }

    private void StartGame()
    {
        BalancePlayers();
        StartRound();
    }

    public void StartRound()
    {
        PlayersManager.Instance.CleanList();
        for (int i = 0; i < ListOfPlayerPerRound[currentRound]; i++)
        {
            PlayersManager.Instance.AddPlayer(listOfPlayers[i]);
        }
        PlayersManager.Instance.StartGame();
    }

    public void EndRound(string winner)
    {
        PlayerIsBalanced = false;
        for (int i = 0; i < ListOfPlayerPerRound[currentRound];i++)
        {
            if (listOfPlayers[0].Author.Equals(winner))
            {
                listOfWinners.Add(listOfPlayers[0]);
            }
            listOfPlayers.RemoveAt(0);
        }
        currentRound++;

        if (currentRound >= numberOfRounds)
            EndBatteryOfRounds();
        else
            PlayerIsBalanced = true;
    }

    public void EndBatteryOfRounds()
    {
        Debug.Log("entrou no final da chave");
        if(listOfWinners.Count == 1)
        {
            //TODO Ganhador
            TwitchChat.Instance.SendFinalMessage(listOfWinners[0].Author);
            return;
        }
        currentRound = 0;
        listOfPlayers = CopyList(listOfWinners);
        listOfWinners.Clear();
        BalancePlayers();
    }

    private List<TwitchCommandData> CopyList(List<TwitchCommandData> copyFrom)
    {
        List<TwitchCommandData> copy = new List<TwitchCommandData>();
        for (int i = 0; i < copyFrom.Count; i++)
        {
            copy.Add(copyFrom[i]);
        }
        return copy;
    }
}
