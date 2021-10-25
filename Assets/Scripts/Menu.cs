using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject menuScreen;

    private void Start()
    {
        ShowCommands();
    }
    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }
    public void BackToLogin()
    {
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
    public CommandCollection cmdCollection;
    private void ShowCommands()
    {
        for (int i = 0; i < cmdCollection.commands.Count; i++)
        {
            GameObject commandAux = Instantiate(commandPrefab, content);
            commandAux.GetComponent<CommandPrefab>().Initialize(cmdCollection.cmdPrefix+ cmdCollection.commands[i].cmd, cmdCollection.commands[i].description);
        }
    }
}
