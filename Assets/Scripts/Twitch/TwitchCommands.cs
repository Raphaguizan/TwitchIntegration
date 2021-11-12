using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Twitch.Chat
{
    [Serializable]
    public class TwitchCommands
    {
        public string cmd = "message";
        public string description = "";
        public bool caseSensitive;
        public UnityEvent<TwitchCommandData> events;
    }

    public struct TwitchCredentials
    {
        public string ChannelName;
        public string Username;
        public string Password;
    };

    [Serializable]
    public struct TwitchCommandData
    {
        public string Author;
        public Color Color;
        public string Message;
    }
}