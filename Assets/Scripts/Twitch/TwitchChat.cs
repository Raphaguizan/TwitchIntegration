using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.IO;
using UnityEngine;
using Game.Util;
using System;

namespace Twitch.Chat
{
    public class TwitchChat : Singleton<TwitchChat>
    {
        // might want to use these while testing with your own information
        // public string Password;
        // public string Username;
        // public string ChannelName;

        public CommandCollection _commands;
        private TwitchCredentials credentials;
        private TcpClient _twitchClient;
        private StreamReader _reader;
        private StreamWriter _writer;

        public static string FinalMessage { get; private set; } = "Parabéns pela vitória {name}!!!";

        //regex
        readonly string messagePattern = @": *!(\w)*";
        readonly string namePattern = @"name=([^;]*)";
        readonly string colorPattern = @"color=([^;]*)";

        readonly string vitoryNamePattern = @"{name}";

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
            if (PlayerPrefs.HasKey(nameof(FinalMessage)))
            {
                FinalMessage = PlayerPrefs.GetString(nameof(FinalMessage));
            }
        }

        void Update()
        {
            if (_twitchClient != null && _twitchClient.Connected)
            {
                ReadChat();
            }
        }

        public static void SetNewCommandCollection(CommandCollection commands)
        {
            Instance._commands = commands;
        }

        public static void Connect(TwitchCredentials cred)
        {
            Instance.credentials = cred;
            Instance._twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
            Instance._reader = new StreamReader(Instance._twitchClient.GetStream());
            Instance._writer = new StreamWriter(Instance._twitchClient.GetStream());

            Instance._writer.WriteLine("PASS " + Instance.credentials.Password);
            Instance._writer.WriteLine("NICK " + Instance.credentials.Username);
            Instance._writer.WriteLine("USER " + Instance.credentials.Username + " 8 * :" + Instance.credentials.Username);
            Instance._writer.WriteLine("CAP REQ :twitch.tv/tags");
            Instance._writer.WriteLine("JOIN #" + Instance.credentials.ChannelName);
            Instance._writer.Flush();
        }

        private void ReadChat()
        {
            if (_twitchClient.Available <= 0) return;

            string message = _reader.ReadLine();
            //Debug.Log(message);

            // Twitch sends a PING message every 5 minutes or so. We MUST respond back with PONG or we will be disconnected 
            if (message.Contains("PING"))
            {
                _writer.WriteLine("PONG");
                _writer.Flush();
                return;
            }
            //change scene when connected
            if (ScenesManager.Instance.ActualScene() == 0 && message.Contains("JOIN #" + credentials.ChannelName))
            {
                ScenesManager.Instance.ChangeScene(1);
            }

            if (message.Contains("PRIVMSG"))
            {
                Debug.Log(message);

                Match userMessageMatch = Regex.Match(message, messagePattern);
                if (userMessageMatch.Success)
                {
                    //get command
                    string command = userMessageMatch.Value.Substring(2);

                    //get command message
                    int messageIndex = userMessageMatch.Index + command.Length + 3;
                    string commandMessage = "";
                    if (messageIndex <= message.Length)
                        commandMessage = message.Substring(messageIndex);

                    //get name
                    Match userNameMatch = Regex.Match(message, namePattern);
                    string author = userNameMatch.Value.Split('=')[1];

                    //get color
                    Match userColorMatch = Regex.Match(message, colorPattern);
                    string colorString = userColorMatch.Value.Split('=')[1];
                    Color newCol;
                    ColorUtility.TryParseHtmlString(colorString, out newCol);

                    _commands.ExecuteCommand(
                        command,
                        new TwitchCommandData
                        {
                            Author = author,
                            Message = commandMessage,
                            Color = newCol
                        });
                }
               
            }
        }

        public static void CopyCommand(TwitchCommandData data)
        {
            SendChatMessage(data.Message);
        }

        public static void SendFinalMessage(string name)
        {
            string finalMessageWithName = Regex.Replace(FinalMessage, Instance.vitoryNamePattern, name);

            SendChatMessage(finalMessageWithName);
        }

        public static void SendChatMessage(string message)
        { 
            Debug.Log($"<color=yellow>{"PRIVMSG #" + Instance.credentials.ChannelName + " :" + message}</color>");
            Instance._writer.WriteLine("PRIVMSG #" + Instance.credentials.ChannelName + " : " + message);
            Instance._writer.Flush();
        }


        public static void SaveFinalMessage(string message)
        {
            FinalMessage = message;
            PlayerPrefs.SetString(nameof(FinalMessage), FinalMessage);
        }

    }

}

