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
    private int currentRound = 0;

    [SerializeField]
    public List<TwitchCommandData> listOfPlayers;
    [SerializeField]
    public List<TwitchCommandData> listOfWinners;

    private List<int> listOfPlayerPerRound;


    private void Start()
    {
        listOfPlayers = new List<TwitchCommandData>();
        listOfWinners = new List<TwitchCommandData>();
        listOfPlayerPerRound = new List<int>();

        StartMenuManager.StartGameEvent += StartGame;
        PlayersManager.vitoriousPlayerEvent += name => EndRound(name);
        //BalancePlayers();
    }

    public void AddPlayer(TwitchCommandData data)
    {
        listOfPlayers.Add(data);
    }

    public void BalancePlayers()
    {
        Debug.Log("aki");
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
            listOfPlayerPerRound.Add(playerPerRound);
            if(restPlayers > 0)
            {
                listOfPlayerPerRound[i]++;
                restPlayers--;
            }
        }
    }

    private void StartGame()
    {
        BalancePlayers();
        StartRound();
    }

    public void StartRound()
    {
        PlayersManager.Instance.CleanList();
        for (int i = 0; i < listOfPlayerPerRound[currentRound]; i++)
        {
            PlayersManager.Instance.AddPlayer(listOfPlayers[i]);
        }
        PlayersManager.Instance.StartGame();
    }

    public void EndRound(string winner)
    {
        for (int i = 0; i < listOfPlayerPerRound[currentRound]; i++)
        {
            if (listOfPlayers[i].Author.Equals(winner))
            {
                listOfWinners.Add(listOfPlayers[i]);
            }
            listOfPlayers.RemoveAt(i);
        }
        currentRound++;

        if (currentRound >= numberOfRounds)
            EndBatteryOfRounds();

    }

    public void EndBatteryOfRounds()
    {
        if(listOfWinners.Count == 1)
        {
            //Todo Ganhador
            TwitchChat.Instance.SendFinalMessage(listOfWinners[0].Author);
            return;
        }
        listOfPlayers = listOfWinners;
        listOfWinners.Clear();
        BalancePlayers();
    }
}
