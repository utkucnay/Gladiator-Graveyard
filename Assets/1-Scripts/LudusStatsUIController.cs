using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LudusStatsUIController : Singleton<LudusStatsUIController>
{
    public GameObject canvasRef;
    public List<GameObject> cardSlots;
    public GameObject movingCardRef;
    private int cardSlotCurrentIndex;
    [SerializeField]private float moveCardAnimationTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuySpecialCard(Card cardToBuy)
    {
        MoveCardToCardBox(cardToBuy);
        foreach (Card c in SpecialCardRandomer.Instance.cardsInShop)
        {
            if(cardToBuy.cardType == c.cardType)
            {
                SpecialCardRandomer.Instance.ReplaceCard(c.cardType);
                return;
            }
        }
    }

    public void MoveCardToCardBox(Card cardToBuy)
    {
        foreach (GameObject g in SpecialCardRandomer.Instance.specialCards)
        {
            if (cardToBuy.cardType == g.GetComponent<CardVisualController>().cardType)
            {
                GameObject copyCard = Instantiate(movingCardRef, canvasRef.transform);
                copyCard.transform.SetParent(canvasRef.transform,false);
                copyCard.GetComponent<RectTransform>().anchorMin = Vector2.one * 0.5f;
                copyCard.GetComponent<RectTransform>().anchorMax = Vector2.one * 0.5f;
                copyCard.GetComponent<RectTransform>().pivot = Vector2.one * 0.5f;

                copyCard.GetComponent<CardVisualController>().cardType = cardToBuy.cardType;
                copyCard.GetComponent<CardVisualController>().UpdateCard(CardController.Instance.GetCardInfo(cardToBuy.cardType));

                Vector2 cardStartPos = canvasRef.transform.InverseTransformPoint(g.transform.position);
                Vector2 cardEndPos = canvasRef.transform.InverseTransformPoint(cardSlots[cardSlotCurrentIndex].transform.position);

                Debug.Log(" transfrom: " + cardStartPos);
                Debug.Log(" transfrom: " + cardEndPos);

                copyCard.GetComponent<RectTransform>().anchoredPosition = canvasRef.transform.InverseTransformPoint(g.transform.position);
                copyCard.GetComponent<RectTransform>().DOAnchorPos(cardEndPos, moveCardAnimationTime).SetEase(Ease.InOutSine).OnComplete(() => ActivateCardSlot(cardToBuy));
                copyCard.transform.DOScale(cardSlots[cardSlotCurrentIndex].transform.localScale, moveCardAnimationTime);
                Destroy(copyCard, moveCardAnimationTime + 0.1f);
                return;
            }
        }
    }

    private void ActivateCardSlot(Card cardToActivate)
    {
        cardSlots[cardSlotCurrentIndex].SetActive(true);
        cardSlots[cardSlotCurrentIndex].GetComponent<CardVisualController>().cardType = cardToActivate.cardType;
        cardSlots[cardSlotCurrentIndex].GetComponent<CardVisualController>().UpdateCard(CardController.Instance.GetCardInfo(cardToActivate.cardType));
        cardSlotCurrentIndex++;
    }
}
