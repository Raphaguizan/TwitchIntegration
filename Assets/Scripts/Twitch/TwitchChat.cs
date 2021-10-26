using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using UnityEngine;
using Game.Util;
using System;

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

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if(_twitchClient != null && _twitchClient.Connected){
            ReadChat();    
        }
    }

    public void SetNewCommandCollection(CommandCollection commands){
         _commands = commands;
    }

    public void Connect(TwitchCredentials cred){
        credentials = cred;
        _twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        _reader = new StreamReader(_twitchClient.GetStream());
        _writer = new StreamWriter(_twitchClient.GetStream());

        _writer.WriteLine("PASS " + credentials.Password);
        _writer.WriteLine("NICK " + credentials.Username);
        _writer.WriteLine("USER " + credentials.Username + " 8 * :" + credentials.Username);
        _writer.WriteLine("JOIN #" + credentials.ChannelName);
        _writer.Flush();
    }

    private void ReadChat(){
        if (_twitchClient.Available <= 0) return;

        string message = _reader.ReadLine();
        Debug.Log(message);
            
        // Twitch sends a PING message every 5 minutes or so. We MUST respond back with PONG or we will be disconnected 
        if (message.Contains("PING")) {
            _writer.WriteLine("PONG");
            _writer.Flush();
            return;
        }
        //change scene when connected
        if (ScenesManager.Instance.ActualScene() == 0 && message.Contains("JOIN #" + credentials.ChannelName))
        {
            ScenesManager.Instance.ChangeScene(1);
        }

        if (message.Contains("PRIVMSG")) {
            var splitPoint = message.IndexOf(_commands.cmdPrefix, 1);
            var author = message.Substring(0, splitPoint);
            author = author.Substring(1);

            // users message
            splitPoint = message.IndexOf(":", 1);
            message = message.Substring(splitPoint + 1);

            if (message.StartsWith(_commands.cmdPrefix)){
                // get the first word
                int index =  message.IndexOf(" ");
                string command = index > -1 ? message.Substring(1, index-1) : message.Substring(1);

                string actualMessage;
                try
                {
                    actualMessage = message.Substring(0 + (_commands.cmdPrefix + command).Length).TrimStart(' ');
                }
                catch(ArgumentOutOfRangeException)
                {
                    actualMessage = "";
                }

                _commands.ExecuteCommand(
                    command,
                    new TwitchCommandData
                    {
                        Author = author,
                        Message = actualMessage
                    });
            }
        } 
    }

    public void WriteChat(TwitchCommandData data)
    {
        //if (_twitchClient.Available <= 0) return;
        Debug.Log($"<color=yellow>{"PRIVMSG #" + credentials.ChannelName + " :" + data.Message}</color>");
        _writer.WriteLine("PRIVMSG #" + credentials.ChannelName+" : "+ data.Message);
        _writer.Flush();
    }

}

    

