using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct PlayerCard
{
    public EquipmentKits equipmentKits;
    public int totalPlayerStats;
    public float str;
    public float dex;
    public float vit;
    public float stamina;
    public float speed;
}

public class CardPlayerController : Singleton<CardPlayerController>
{
    [SerializeField]BasePlayerAttributes basePlayerAttributes;
    [SerializeField]BasePlayerAttributes playerAttributes;


    [SerializeField] public int minTotalStat, maxTotalStat;
    [SerializeField] public int minStat, maxStat;

    public PlayerCard? selectedCard;

    GladiatorInfoCardController[] cards;

    private void Awake()
    {
        cards = GetComponentsInChildren<GladiatorInfoCardController>();
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].basePlayerAttributes = basePlayerAttributes;
            cards[i].playerAttributes = playerAttributes;
        }
    }


    public void SelectCardInactive(GladiatorInfoCardController card)
    {
        List<GladiatorInfoCardController> cards = new List<GladiatorInfoCardController>(this.cards);
        cards.Remove(card);
        card.GetComponent<Button>().interactable = false;
        foreach (var otherCard in cards)
        {
            otherCard.GetComponent<Button>().interactable = true;
        }
    }

    public void StartRunButtonClicked()
    {
        if (selectedCard != null)
        {
            GameController.Instance.equippedKit = selectedCard.Value.equipmentKits;
            GameController.Instance.InvokeRun();
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Select Card");
        }
    }
}
