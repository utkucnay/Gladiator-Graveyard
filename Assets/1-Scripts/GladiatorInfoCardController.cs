using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GladiatorInfoCardController : MonoBehaviour
{
    [HideInInspector] public BasePlayerAttributes basePlayerAttributes;
    [HideInInspector] public BasePlayerAttributes playerAttributes;


    [SerializeField] private TextMeshProUGUI totalPlayerStats;
    [SerializeField] private TextMeshProUGUI className;

    [SerializeField] private TextMeshProUGUI vitality;
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI stamina;
    [SerializeField] private TextMeshProUGUI speed;
    [SerializeField] private TextMeshProUGUI dexterity;


    [SerializeField] private GameObject root;
    [SerializeField] private GameObject dot;
    [SerializeField] private GameObject dot1;
    [SerializeField] private GameObject dot2;
    [SerializeField] private GameObject dot3;
    [SerializeField] private GameObject dot4;

    [SerializeField] private LineRenderer lineRendererRef;

    // Start is called before the first frame update
    void Start()
    {
        var BaseStats = new List<float>();
        BaseStats.Add(basePlayerAttributes.str);
        BaseStats.Add(basePlayerAttributes.dex);
        BaseStats.Add(basePlayerAttributes.vit);
        BaseStats.Add(basePlayerAttributes.stam);
        BaseStats.Add(basePlayerAttributes.spd);

        var playerCard = CreatePlayerCards(BaseStats);

        SetCard(playerCard);

        CreatePentagon(playerCard);
    }

    PlayerCard CreatePlayerCards(List<float> BaseStats)
    {
        List<int> allRandom = new List<int>();
        var randTotalPlayerStats = Random.Range(CardPlayerController.Instance.minTotalStat, CardPlayerController.Instance.maxTotalStat);
        int oldMultiplerBaseStats = 0;
        int totalPlayerStats = 100;
        totalPlayerStats += totalPlayerStats * randTotalPlayerStats / 100;

        for (int i = 0; i < 5; i++)
        {
            if (i < 2)
            {
                var PlayerStat = Random.Range(CardPlayerController.Instance.minStat, CardPlayerController.Instance.maxStat);
                BaseStats[i] += BaseStats[i] * PlayerStat / 100;
                oldMultiplerBaseStats += PlayerStat;
                allRandom.Add(PlayerStat);
            }
            else if (i < 4)
            {
                var PlayerStat = Random.Range(Mathf.Clamp(CardPlayerController.Instance.minStat - oldMultiplerBaseStats + randTotalPlayerStats, CardPlayerController.Instance.minStat, CardPlayerController.Instance.maxStat), 
                    Mathf.Clamp(CardPlayerController.Instance.maxStat - oldMultiplerBaseStats + randTotalPlayerStats, CardPlayerController.Instance.minStat, CardPlayerController.Instance.maxStat));
                oldMultiplerBaseStats += PlayerStat;
                BaseStats[i] += BaseStats[i] * PlayerStat / 100;
                allRandom.Add(PlayerStat);
            }
            else
            {
                BaseStats[i] += BaseStats[i] * (randTotalPlayerStats - oldMultiplerBaseStats) / 100;
                allRandom.Add((randTotalPlayerStats - oldMultiplerBaseStats));
            }
        }

        PlayerCard playerCard = new PlayerCard();
        playerCard.str = BaseStats[0];
        playerCard.dex = BaseStats[1];
        playerCard.vit = BaseStats[2];
        playerCard.stamina = BaseStats[3];
        playerCard.speed = BaseStats[4];
        playerCard.totalPlayerStats = totalPlayerStats;
        playerCard.equipmentKits = EquipmentKits.Murmillo;


        string card = "";

        for (int i = 0; i < 5; i++)
        {
            card += "Stats :" + BaseStats[i] + " ";
        }
        card += " \n ";
        int Multipler = 0;
        for (int i = 0; i < 5; i++)
        {
            card += "Multipler :" + allRandom[i] + " ";
            Multipler += allRandom[i];
        }

        card += "\n total preditced Multipler :" + randTotalPlayerStats;
        card += " total Multipler :" + Multipler;
        Debug.Log(card);

        return playerCard;
    }

    void SetCard(PlayerCard playerCard)
    {
        var texts = GetComponentsInChildren<TextMeshProUGUI>();
        vitality.text = ": " + playerCard.vit;
        stamina.text = ": " + playerCard.stamina;
        strength.text = ": " + playerCard.str;
        dexterity.text = ": " + playerCard.dex;
        speed.text = ": " + playerCard.speed;
        totalPlayerStats.text = "Total Stats : " + playerCard.totalPlayerStats + "";
        className.text = "Class : " + playerCard.equipmentKits.ToString();

        GetComponent<Button>().onClick.AddListener(() => {
            SetPlayerArttibutes(playerCard);
           CardPlayerController.Instance.selectedCard = playerCard;
        });
    }

    void SetPlayerArttibutes(PlayerCard playerCard)
    {
        playerAttributes.spd = playerCard.speed;
        playerAttributes.stam = playerCard.stamina;
        playerAttributes.str = playerCard.str;
        playerAttributes.dex = playerCard.dex;
        playerAttributes.vit = playerCard.vit;
    }

    private void CreatePentagon(PlayerCard card)
    {
        dot.GetComponent<RectTransform>().localPosition = Quaternion.Euler(0, 0, 0) * Vector3.up * 30 + Quaternion.Euler(0, 0, 0) * Vector3.up * Scale(
            CardPlayerController.Instance.minStat, CardPlayerController.Instance.maxStat, -15, 15, GetPercentChange(card.str, basePlayerAttributes.str));
        dot1.GetComponent<RectTransform>().localPosition = Quaternion.Euler(0, 0, 72) * Vector3.up * 30 + Quaternion.Euler(0, 0, 72) * Vector3.up * Scale(
            CardPlayerController.Instance.minStat, CardPlayerController.Instance.maxStat, -15, 15, GetPercentChange(card.dex, basePlayerAttributes.dex));
        dot2.GetComponent<RectTransform>().localPosition = Quaternion.Euler(0, 0, 72 * 2) * Vector3.up * 30 + Quaternion.Euler(0, 0, 72 * 2) * Vector3.up * Scale(
            CardPlayerController.Instance.minStat, CardPlayerController.Instance.maxStat, -15, 15, GetPercentChange(card.stamina, basePlayerAttributes.stam));
        dot3.GetComponent<RectTransform>().localPosition = Quaternion.Euler(0, 0, 72 * 3) * Vector3.up * 30 + Quaternion.Euler(0, 0, 72 * 3) * Vector3.up * Scale(
            CardPlayerController.Instance.minStat, CardPlayerController.Instance.maxStat, -15, 15, GetPercentChange(card.vit, basePlayerAttributes.vit));
        dot4.GetComponent<RectTransform>().localPosition = Quaternion.Euler(0, 0, 72 * 4) * Vector3.up * 30 + Quaternion.Euler(0, 0, 72 * 4) * Vector3.up * Scale(
            CardPlayerController.Instance.minStat, CardPlayerController.Instance.maxStat, -15, 15, GetPercentChange(card.speed, basePlayerAttributes.spd));

        Vector3[] pos = new Vector3[] { 
            dot.transform.localPosition,
            dot1.transform.localPosition,
            dot2.transform.localPosition,
            dot3.transform.localPosition,
            dot4.transform.localPosition,
            dot.transform.localPosition,
        };

        lineRendererRef.SetPositions(pos);
    }
    private float GetPercentChange(float cardVal, float baseVal)
    {
        return ((cardVal / baseVal) - 1) * 100;

    }

    public float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    
}
