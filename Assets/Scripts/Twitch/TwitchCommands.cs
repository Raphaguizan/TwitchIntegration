using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TwitchCommands
{
    public string cmd = "message";
    public string description = "";
    public UnityEvent<TwitchCommandData> events;
}

public struct TwitchCredentials
{
    public string ChannelName;
    public string Username;
    public string Password;
};

public struct TwitchCommandData
{
    public string Author;
    public Color Color;
    public string Message;
}
