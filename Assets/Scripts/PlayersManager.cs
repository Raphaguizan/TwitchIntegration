using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public int playerLimit = 3;
    public List<Player> players;

    public void AddPlayer(TwitchCommandData data)
    {
        if (players.Count > playerLimit) return;

        if (!Duplicate(data.Author))
        {
            Player aux = new Player(data.Author);
            players.Add(aux);
        }
    }

    private bool Duplicate(string playerName)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerName.Equals(playerName))
            {
                return true;
            }
        }
        return false;
    }
}
