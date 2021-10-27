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

        //regex
        readonly string messagePattern = @": *!(\w)*";
        readonly string namePattern = @"name=([^;]*)";
        readonly string colorPattern = @"color=([^;]*)";

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            if (_twitchClient != null && _twitchClient.Connected)
            {
                ReadChat();
            }
        }

        public void SetNewCommandCollection(CommandCollection commands)
        {
            _commands = commands;
        }

        public void Connect(TwitchCredentials cred)
        {
            credentials = cred;
            _twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
            _reader = new StreamReader(_twitchClient.GetStream());
            _writer = new StreamWriter(_twitchClient.GetStream());

            _writer.WriteLine("PASS " + credentials.Password);
            _writer.WriteLine("NICK " + credentials.Username);
            _writer.WriteLine("USER " + credentials.Username + " 8 * :" + credentials.Username);
            _writer.WriteLine("CAP REQ :twitch.tv/tags");
            _writer.WriteLine("JOIN #" + credentials.ChannelName);
            _writer.Flush();
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
                //var splitPoint = message.IndexOf("name=");
                //var nameString = message.Substring(splitPoint+5);
                //var cutTag = nameString.IndexOf(";");
                //var author = nameString.Substring(0,cutTag);
                //Debug.Log(author);


                //splitPoint = message.IndexOf("color=");
                //nameString = message.Substring(splitPoint + 6);
                //cutTag = nameString.IndexOf(";");
                //var colorString = nameString.Substring(0, cutTag);
                //Color newCol;
                //ColorUtility.TryParseHtmlString(colorString, out newCol);

                //Debug.Log(newCol);

                // users message
                //splitPoint = message.IndexOf(":", 1);
                //message = message.Substring(splitPoint + 1);

                //Debug.Log(message);

                //if (message.StartsWith(_commands.cmdPrefix)){
                //    // get the first word
                //    int index =  message.IndexOf(" ");
                //    string command = index > -1 ? message.Substring(1, index-1) : message.Substring(1);

                //    string actualMessage;
                //    try
                //    {
                //        actualMessage = message.Substring(0 + (_commands.cmdPrefix + command).Length).TrimStart(' ');
                //    }
                //    catch(ArgumentOutOfRangeException)
                //    {
                //        actualMessage = "";
                //    }

                //    int colorIndex = message.IndexOf("color=");
                //    string hexColor = message.Substring(colorIndex, colorIndex + 7);
                //    Debug.Log(hexColor);
                //    Debug.Log(actualMessage);

                //    _commands.ExecuteCommand(
                //        command,
                //        new TwitchCommandData
                //        {
                //            //Author = author,
                //            Message = actualMessage
                //        });
                //}
            }
        }

        public void WriteChat(TwitchCommandData data)
        {
            //if (_twitchClient.Available <= 0) return;
            Debug.Log($"<color=yellow>{"PRIVMSG #" + credentials.ChannelName + " :" + data.Message}</color>");
            _writer.WriteLine("PRIVMSG #" + credentials.ChannelName + " : " + data.Message);
            _writer.Flush();
        }

    }

}

