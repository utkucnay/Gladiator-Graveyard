using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class GameController : Singleton<GameController>
{

    [HideInInspector]public UnityEvent runStarted;
    [HideInInspector]public UnityEvent runEnded;
    [HideInInspector]public UnityEvent playerDied;
    [HideInInspector] public UnityEvent playerBorn;
    [HideInInspector]public EquipmentKits equippedKit;

    Arena[] Arenas;
    public Arena CurrentArena;

    public GameObject Death;
    public GameObject MainMenu;

    public override void Awake()
    {
        base.Awake();
        Arenas = new Arena[1];
        Boundary boundary = new Boundary(8.7f,14.2f,-6.5f,-14);
        Arena arena = new Arena(boundary);
        Arenas[0] = arena;
        CurrentArena = Arenas[0];
        playerDied.AddListener(()=> {
            Death.SetActive(true);
            Glory.RemoveGlory(Glory.GetActualGlory());
        });
    }

    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokePlayerDied()
    {
        playerDied?.Invoke();
    }
  
    public void InvokeRun()
    {
        runStarted?.Invoke();
    }

    public void InvokeRunEnded()
    {
        runEnded?.Invoke();
    }

    public void PlayerBornEvent()
    {
        playerBorn?.Invoke();
    }

    public void RestartGame() {
        WaveController.Instance.ClearTrash();        
        WaveController.Instance.RestartGame();
        CardController.Instance.Start();
        CardController.Instance.ResetProp();
        
        MainMenu.SetActive(true);
    }
}
