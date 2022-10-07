using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : Singleton<MainMenuUIController>
{
    [Header("Kit Button References")]
    [SerializeField]
    private GameObject MurmilloKitButton;
    [SerializeField]
    private GameObject DimachaerusKitButton;
    [SerializeField]
    private GameObject SagittariusKitButton;

    public override void Start()
    {
        base.Start();
        SetKitButtons();
    }
    public override void RunStarted()
    {
        base.RunStarted();
        gameObject.SetActive(false);
    }
    public override void RunEnded()
    {
        base.RunEnded();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    private void SetKitButtons()
    {
        int openKits = SaveController.Instance.GetOpenKits();

        DimachaerusKitButton.GetComponent<Button>().enabled = false;
        SagittariusKitButton.GetComponent<Button>().enabled = false;

        if (openKits > 0)
        {
            DimachaerusKitButton.GetComponent<Button>().enabled = true;
        }
        if (openKits > 1)
        {
            SagittariusKitButton.GetComponent<Button>().enabled = true;
        }
    }

    public void StartRunButtonClicked()
    {
        GameController.Instance.equippedKit = EquipmentKits.Murmillo;
        GameController.Instance.InvokeRun();
    }

   
}
