using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardVisualController : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI details;
    public TextMeshProUGUI cost;

    private Image cardImage;

    [SerializeField]
    public CardType cardType;

     void Awake()
    {
        cardImage = gameObject.GetComponent<Image>();
    }

   

    void Start()
    {
        UpdateCard(CardController.Instance.GetCardInfo(cardType));
    }

    public void CardPressed()
    {
        CardController.Instance.BuyCard(cardType);
        UpdateCard(CardController.Instance.GetCardInfo(cardType));
    }

    public void UpdateCard(CardInfo cardInfo)
    {
        cost.text = cardInfo.cost.ToString();
        details.text = cardInfo.details.ToString();
        title.text = cardInfo.title.ToString();

        if(cardImage == null)
        {
            cardImage = gameObject.GetComponent<Image>();
        }
        cardImage.sprite = cardInfo.cardImage;
    }
}
