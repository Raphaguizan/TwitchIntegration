using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
    [Header("Animations tags")]
    public string run = "run";
    public string charging = "charge";
    public string celebrate1 = "celebrating_1";
    public string celebrate2 = "celebrating_2";
    public string fall1 = "fall_1";
    public string fall2 = "fall_2";
    public string win = "win";
    public string lose = "lose";

    private Animator myAnim;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    public void TriggerRun(bool value)
    {
        myAnim.SetBool(run, value);
    }
    public void TriggerCharge()
    {
        myAnim.SetTrigger(charging);
    }
    public void TriggerFall()
    {
        if (Random.value > 0.5f)
        {
            myAnim.SetTrigger(fall1);
        }
        else
        {
            myAnim.SetTrigger(fall2);
        }
    }
    public void TriggerCelebrate()
    {
        if(Random.value > 0.5f)
        {
            myAnim.SetTrigger(celebrate1);
        }
        else
        {
            myAnim.SetTrigger(celebrate2);
        }
    }
    public void TriggerWin()
    {
        myAnim.SetTrigger(win);
    }
    public void TriggerLose()
    {
        myAnim.SetTrigger(lose);
    }
}
