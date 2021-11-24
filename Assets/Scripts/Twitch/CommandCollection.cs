using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;

namespace Twitch.Chat
{
    public class CommandCollection : Singleton<CommandCollection>
    {
        public string cmdPrefix = "!";
        public List<TwitchCommands> commands;

        public static bool Initialized = false;

        private void Start()
        {
            StartCoroutine(Initiallize());
        }

        IEnumerator Initiallize()
        {
            yield return new WaitUntil(() =>TwitchChat.Instance != null);
            for (int i = 0; i < commands.Count; i++)
            {
                if (PlayerPrefs.HasKey("Command"+i))
                {
                    commands[i].cmd = PlayerPrefs.GetString("Command"+i);
                }
                if (PlayerPrefs.HasKey("CommandCS"+i))
                {
                    commands[i].caseSensitive = PlayerPrefs.GetInt("CommandCS"+i) == 1? true:false;
                }
            }
            TwitchChat.SetNewCommandCollection(this);
            Initialized = true;
        }

        public void ExecuteCommand(string commandString, TwitchCommandData data)
        {
            TwitchCommands command = FindCommand(commandString).command;
            if (command != null)
            {
                command.events?.Invoke(data);
            }
        }

        public bool ChangeCommandName(string oldName, string newName)
        {
            (TwitchCommands command, int index) comandResp = FindCommand(oldName);
            TwitchCommands command = comandResp.command;
            if (command != null && !HasEqualCommands(newName))
            {
                command.cmd = newName;
                PlayerPrefs.SetString("Command" + comandResp.index, newName);
                return true;
            }
            Debug.LogError("comando antigo invlálido ou nome novo repetido");
            return false;
        }

        public bool ChangeCommandSensitive(string commandString, bool caseSens)
        {
            (TwitchCommands command, int index) comandResp = FindCommand(commandString);
            TwitchCommands command = comandResp.command;
            if (command != null)
            {
                command.caseSensitive = caseSens;
                PlayerPrefs.SetInt("CommandCS" + comandResp.index, caseSens?1:0);
                return true;
            }
            return false;
        }


        private (TwitchCommands command, int index) FindCommand(string name)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                if (!commands[i].caseSensitive)
                {
                    if (commands[i].cmd.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return (commands[i], i);
                    }
                }
                else
                {
                    if (commands[i].cmd.Equals(name, StringComparison.CurrentCulture))
                    {
                        return (commands[i], i);
                    }
                }
            }
            Debug.LogError("commando inexistente");
            return (null, -1);
        }

        private bool HasEqualCommands(string name)
        {
            int count = 0;
            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].cmd.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    count++;
                }
            }
            return count > 1;
        }
    }
}
