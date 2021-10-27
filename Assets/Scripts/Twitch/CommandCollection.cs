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

        private void Start()
        {
            StartCoroutine(Initiallize());
        }

        IEnumerator Initiallize()
        {
            yield return new WaitUntil(() =>TwitchChat.Instance != null);
            TwitchChat.Instance.SetNewCommandCollection(this);
        }

        public void ExecuteCommand(string commandString, TwitchCommandData data)
        {
            TwitchCommands command = FindCommand(commandString);
            if (command != null)
            {
                command.events?.Invoke(data);
            }
        }

        public bool ChangeCommandName(string oldName, string newName)
        {
            TwitchCommands command = FindCommand(oldName);
            if (command != null && !HasEqualCommands(newName))
            {
                command.cmd = newName;
                return true;
            }
            Debug.LogError("comando antigo invlálido ou nome novo repetido");
            return false;
        }

        public bool ChangeCommandSensitive(string commandString, bool caseSens)
        {
            TwitchCommands command = FindCommand(commandString);
            if (command != null)
            {
                command.caseSensitive = caseSens;
                return true;
            }
            return false;
        }


        private TwitchCommands FindCommand(string name)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                if (!commands[i].caseSensitive)
                {
                    if (commands[i].cmd.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return commands[i];
                    }
                }
                else
                {
                    if (commands[i].cmd.Equals(name, StringComparison.CurrentCulture))
                    {
                        return commands[i];
                    }
                }
            }
            Debug.LogError("commando inexistente");
            return null;
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
