using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Twitch.Chat;

public class CommandPrefab : MonoBehaviour
{
    public TMP_InputField commandInput;
    public TextMeshProUGUI description;
    public TextMeshProUGUI AlertText;
    public Toggle caseSensitive;

    private TwitchCommands command;

    public void Initialize(TwitchCommands command)
    {
        this.command = command;
        commandInput.text = command.cmd;
        description.text = command.description;
        caseSensitive.isOn = command.caseSensitive;
        AlertText.text = "";
    }

    public void ChangeCommandName(string newName)
    {
        if (!CommandCollection.Instance.ChangeCommandName(command.cmd, newName))
        {
            commandInput.text = command.cmd;
            AlertText.text = "Erro ao mudar o commando";
        }
        else
        {
            AlertText.text = "";
        }
    }

    public void ChangeSensitive(bool val)
    {
        if(!CommandCollection.Instance.ChangeCommandSensitive(command.cmd, val))
        {
            caseSensitive.isOn = command.caseSensitive;
            AlertText.text = "Erro ao mudar case sensitive";
        }
        else
        {
            AlertText.text = "";
        }

    }
}
