using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public int playerLimit = 10;
    public List<GameObject> players;
    public GameObject playerPrefab;

    private readonly int InitialLayerNum = 6;

    private void Start()
    {
        players = new List<GameObject>();
    }

    public void AddPlayer(TwitchCommandData data)
    {
        var aux = Instantiate(playerPrefab, transform);
        aux.transform.position = new Vector3(transform.position.x + 4 * players.Count, transform.position.y, transform.position.z);
        aux.GetComponent<Player>().Initialize(data.Author);
        players.Add(aux);
        CameraAdjust();
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
}
