using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    public GameObject mainMenuUI;
    public GameObject ludusUI;
    public GameObject tutorialUI;
    public GameObject gameFinishUI;

    private void Start()
    {
        WaveController.Instance.ArenaMatchesFinished.AddListener(SwitchToGameFinishUI);
    }

    public void SwitchToTutorialUI()
    {
        mainMenuUI.SetActive(false);
        tutorialUI.SetActive(true);
    }

    public void SwitchToGame()
    {
        mainMenuUI.SetActive(false);
        tutorialUI.SetActive(false);
        ludusUI.SetActive(false);
        gameFinishUI.SetActive(false);

    }

    public void SwitchToGameFinishUI()
    {
        mainMenuUI.SetActive(false);
        tutorialUI.SetActive(false);
        ludusUI.SetActive(false);
        gameFinishUI.SetActive(true);
    }


}
