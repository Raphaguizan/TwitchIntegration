using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;

public class RPGPlayer : Singleton<RPGPlayer>
{
    public int CurrentXP = 0;

    void Awake(){
        DontDestroyOnLoad(this);
        CurrentXP = 0;
    }

    public void AddXP(int amount){
        CurrentXP += amount;
        Debug.Log($"<color=green>RPG Player's current xp is: {CurrentXP}</color>");
    }
}
