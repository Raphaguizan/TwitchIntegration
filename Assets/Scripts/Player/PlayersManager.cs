using System;
using System.Collections.Generic;
using UnityEngine;
using Twitch.Chat;
using Game.Util;

public class PlayersManager : Singleton<PlayersManager>
{
    public List<GameObject> players;
    public GameObject playerPrefab;
    public Transform finishLine;

    public Vector2 fillAdd;

    private readonly int InitialLayerNum = 6;

    public static Action<string> vitoriousPlayerEvent;
    public static Action StartGameEvent;
    public static bool IsPlaying;

    private void Start()
    {
        players = new List<GameObject>();
        IsPlaying = false;
    }

    public void StartGame()
    {
        StartGameEvent?.Invoke();
        IsPlaying = true;
    }

    #region player commands
    public void AddPlayer(TwitchCommandData data)
    {
        var aux = Instantiate(playerPrefab, transform);
        aux.transform.position = new Vector3(transform.position.x + 4 * players.Count, transform.position.y, transform.position.z);
        aux.GetComponent<Player>().Initialize(data.Author, data.Color, finishLine.position);
        players.Add(aux);
        CameraAdjust();
    }
    public void AddCharge(TwitchCommandData data)
    {
        for (int i = 0; i < players.Count; i++)
        {
            Player indexPlayer = players[i].GetComponent<Player>();
            if (indexPlayer.playerName.Equals(data.Author))
            {
                float fillToAdd = UnityEngine.Random.Range(fillAdd.x, fillAdd.y);
                indexPlayer.AddFill(fillToAdd, (fillToAdd/fillAdd.y) >= .9f);
                return;
            }
        }
    }
    public void StartRun(TwitchCommandData data)
    {
        for (int i = 0; i < players.Count; i++)
        {
            Player indexPlayer = players[i].GetComponent<Player>();
            if (indexPlayer.playerName.Equals(data.Author))
            {
                indexPlayer.StartRun();
                return;
            }
        }
    }
    #endregion

    public void ChampionPlayer(string name)
    {
        IsPlaying = false;
        vitoriousPlayerEvent?.Invoke(name);
    }

    private void CameraAdjust()
    {
        float screenSize = 1 / (float)players.Count;
        for (int i = 0; i < players.Count; i++)
        {
            Camera playerCam = players[i].GetComponent<Player>().GetPlayerCamera();
            players[i].GetComponent<Player>().ChangeVirtualCameraLayer(InitialLayerNum + i);
            playerCam.gameObject.layer = InitialLayerNum + i; 
            playerCam.cullingMask = LayerMask.GetMask("Default", "TransparentFX","Ignore Raycast", "Water", "UI","Player"+i);
            playerCam.rect = new Rect(screenSize*i, 0, screenSize, 1);
        }
    }

    public void CleanList()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetActive(false);
            Destroy(players[i]);
        }
        players.Clear();
    }

}
