using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Card 
{
    public Card(string name, Sprite image, string definition, UnityAction call)
    {
        Name = name;
        Image = image;
        Definition = definition;
        AddEventListener(call);
    }
    public Card(CardType cardType, string name, Sprite image, string definition, UnityAction call, int cost)
    {
        Name = name;
        Image = image;
        Definition = definition;
        AddEventListener(call);
        BaseCost = cost;
        Cost = cost;
        this.cardType = cardType;
    }
    public Card(string name, Sprite image)
    {
        Name = name;
        Image = image;
    }

    public CardType cardType{ get; set; }

    public string Name { get; set; }
    public Sprite Image { get; set; }
    public string Definition { get; set; }
    public int Cost { get; set; }
    public bool available { get; set; }

    public int BaseCost { get; set; }

    UnityEvent PickCard = new UnityEvent();

    public void AddEventListener(UnityAction call)
    {
        PickCard.AddListener(call);
    }

    public void InvokeEvent()
    {
        if (PickCard != null)
        {
            PickCard.Invoke();
        }
    }
}
