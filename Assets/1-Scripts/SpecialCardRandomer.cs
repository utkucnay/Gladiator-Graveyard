using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialCardRandomer : Singleton<SpecialCardRandomer>
{
    public GameObject[] specialCards;
    [HideInInspector] public List<Card> cardsInShop;

    private void Awake()
    {
        cardsInShop = new List<Card>();
    }

    private void OnEnable()
    {
        if (CardController.Instance.SpecialMurmilloCards != null)
        {
            RandomizeSpecialPool();
        }
    }
    
    public void ReplaceCard(CardType cardToReplace)
    {
        int index = 0;
        foreach(GameObject g in specialCards)
        {
            if(g.GetComponent<CardVisualController>().cardType == cardToReplace)
            {
                if(CardController.Instance.SpecialMurmilloCards.Count >= specialCards.Length)
                {
                    List<Card> randomCardList = new List<Card>(GetRandomCards(1, true));
                    CardVisualController cardVisualContRef = g.GetComponent<CardVisualController>();

                    cardsInShop[index] = randomCardList[0];
                    cardVisualContRef.cardType = randomCardList[0].cardType;
                    cardVisualContRef.UpdateCard(CardController.Instance.GetCardInfo(cardVisualContRef.cardType));
                }
                else
                {
                    HideCard(g);
                }
                return;
            }
            index++;
        }
    }

    private void HideCard(GameObject card)
    {
        card.GetComponent<Image>().enabled = false;
        card.GetComponent<Button>().enabled = false;
        foreach (TextMeshProUGUI t in card.GetComponentsInChildren<TextMeshProUGUI>())
        {
            t.enabled = false;
        }
        foreach (Image i in card.GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }
    }

    public void RandomizeSpecialPool()
    {
        cardsInShop.Clear();
        List<Card> randomCardList = GetRandomCards(specialCards.Length, false);

        int index = 0;
        foreach(GameObject g in specialCards)
        {
            Debug.Log("name: " + randomCardList[index].Name + " type " + randomCardList[index].cardType);
            CardVisualController cardVisualContRef = g.GetComponent<CardVisualController>();
            if(cardVisualContRef != null)
            {
                cardsInShop.Add(randomCardList[index]);
                cardVisualContRef.cardType = randomCardList[index].cardType;
                cardVisualContRef.UpdateCard(CardController.Instance.GetCardInfo(cardVisualContRef.cardType));
            }
            index++;
        }
    }

    private bool ListContainsCard(CardType cardType, List<Card> cardList)
    {
        foreach(Card c in cardList)
        {
            if (c.cardType == cardType)
            {
                return true;
            }
        }
        return false;
    }

    public List<Card> GetRandomCards(int numberOfCards, bool shouldFilter)
    {
        List<Card> cardListToRandom = new List<Card>(CardController.Instance.SpecialMurmilloCards);
        List<Card> cardListToReturn = new List<Card>();

        switch (GameController.Instance.equippedKit)
        {
            case EquipmentKits.Murmillo:
                cardListToRandom = new List<Card>(CardController.Instance.SpecialMurmilloCards);
                break;
            case EquipmentKits.Dimachaerus:
                break;
            case EquipmentKits.Sagittarius:
                break;
            default:
                break;
        }

        // filter cards with existing ones
        if (shouldFilter)
        {
            for (int i =0; i<cardListToRandom.Count;i++)
            {
                if (ListContainsCard(cardListToRandom[i].cardType, cardsInShop))
                {
                    cardListToRandom.RemoveAt(i);
                    i--;
                }
            }
        }

        GeneralPurposeFunctions.Shuffle(cardListToRandom);

        for(int i = 0; i < numberOfCards; i++)
        {
            Debug.Log(cardListToRandom[i].Name);
            if(i<cardListToRandom.Count)
            {
                cardListToReturn.Add(cardListToRandom[i]);
            }
        }

        return cardListToReturn;
    }
   
}
