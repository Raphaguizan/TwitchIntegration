using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twitch.Chat;
using Game.Util;
using TMPro;
public class Menu : MonoBehaviour
{
    public GameObject menuScreen;
    public SO_Int secondsToAutoStart;

    private void Start()
    {
        StartCoroutine(Initialize());
    }
    IEnumerator Initialize()
    {
        yield return new WaitUntil(() => CommandCollection.Initialized);

        if (PlayerPrefs.HasKey("autoStart"))
            secondsToAutoStart.value = PlayerPrefs.GetInt("autoStart");

        ShowCommands();
        finalMessageObj.text = TwitchChat.FinalMessage;
        autoStartObj.text = secondsToAutoStart.value.ToString();
    }
    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }
    public void BackToLogin()
    {
        Time.timeScale = 1;
        ScenesManager.Instance.ChangeScene(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuScreen.SetActive(!menuScreen.activeInHierarchy);
            if (menuScreen.activeInHierarchy)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    [Space]
    public Transform content;
    public GameObject commandPrefab;
    private void ShowCommands()
    {
        for (int i = 0; i < CommandCollection.Instance.commands.Count; i++)
        {
            GameObject commandAux = Instantiate(commandPrefab, content);
            commandAux.GetComponent<CommandPrefab>().Initialize(CommandCollection.Instance.commands[i]);
        }
    }

    [Space]
    public TMP_InputField finalMessageObj;

    public void SaveFinalMessage(string message)
    {
        TwitchChat.SaveFinalMessage(message);
    }

    [Space]
    public TMP_InputField autoStartObj;

    public void SaveAutoStartSeconds(string seconds)
    {
        int intSecond = int.Parse(seconds);
        secondsToAutoStart.value = intSecond;
        PlayerPrefs.SetInt("autoStart", intSecond);
    }
}
