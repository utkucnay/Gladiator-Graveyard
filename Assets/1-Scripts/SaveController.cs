using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : Singleton<SaveController>
{
    [Header("Saved Variables")]
    [ReadOnly][SerializeField]
    int openKits;

    public override void Awake()
    {
        base.Awake();

        InitializePlayerPrefs();
    }
    public override void Start()
    {
        base.Start();
    }

    public override void RunStarted()
    {
        base.RunStarted();
    }

    public override void RunEnded()
    {
        base.RunEnded();
    }

    private void InitializePlayerPrefs()
    {
        if(!PlayerPrefs.HasKey("OpenKits"))
        {
            PlayerPrefs.SetInt("OpenKits", 0);
        }
    }

    [Button]
    private void RefreshShownData()
    {
        openKits = GetOpenKits();
    }

    [Button("Delete All Data (DANGER)")]
    public void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    public int GetOpenKits()
    {
        return PlayerPrefs.GetInt("OpenKits");
    }

   
}
