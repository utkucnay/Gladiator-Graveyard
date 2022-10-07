using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    public virtual void Start()
    {
        GameController.Instance.runStarted.AddListener(RunStarted);
        GameController.Instance.runEnded.AddListener(RunEnded);
        GameController.Instance.playerDied.AddListener(PlayerDied);
        GameController.Instance.playerBorn.AddListener(PlayerBorn);
    }

    public virtual void PlayerDied()
    {
       
    }

    public virtual void PlayerBorn()
    {

    }

    public virtual void RunEnded()
    {

    }

    public virtual void RunStarted()
    {

    }
}
