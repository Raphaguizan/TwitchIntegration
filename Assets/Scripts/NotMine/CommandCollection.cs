using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCollection : MonoBehaviour
{
    public string cmdPrefix = "!";
    public List<TwitchCommands> commands;

    private void Start()
    {
        StartCoroutine(Initiallize());
    }

    IEnumerator Initiallize()
    {
        yield return new WaitUntil(() =>TwitchChat.Instance != null);
        TwitchChat.Instance.SetNewCommandCollection(this);
    }

    public void ExecuteCommand(string command, TwitchCommandData data)
    {
        for (int i = 0; i < commands.Count; i++)
        {
            if (commands[i].cmd.Equals(command))
            {
                commands[i].events?.Invoke(data);
                return;
            }
        }
        Debug.LogError("commando inexistente");
    }
}
