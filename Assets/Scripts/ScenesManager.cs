using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;
using UnityEngine.SceneManagement;

public class ScenesManager : Singleton<ScenesManager>
{
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public int ActualScene()
    {
       return SceneManager.GetActiveScene().buildIndex;
    }
}
