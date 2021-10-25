using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandPrefab : MonoBehaviour
{
    public TextMeshProUGUI command;
    public TextMeshProUGUI description;

    public void Initialize(string cmd, string desc)
    {
        command.text = cmd;
        description.text = desc;
    }
}
