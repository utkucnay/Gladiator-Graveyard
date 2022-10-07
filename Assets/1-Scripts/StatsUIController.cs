using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUIController : Singleton<StatsUIController>
{
    public BasePlayerAttributes basePlayerAttributesRef;
    
    public TextMeshProUGUI vitText;
    public TextMeshProUGUI strText;
    public TextMeshProUGUI stamText;
    public TextMeshProUGUI spdText;
    public TextMeshProUGUI dexText;

    private GameObject playerRef;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        UpdateStatTexts();
    }

    public void UpdateStatTexts()
    {
        if(playerRef ==null)
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");
        }
        vitText.SetText(": " + playerRef.GetComponent<PlayerHealth>().maxHealth.ToString("0"));
        strText.SetText(": " + (PlayerCharacterCombat.Instance.DamageMultiplier * basePlayerAttributesRef.str).ToString("0.0"));
        stamText.SetText(": " + PlayerStamina.Instance.GetMaxStamina().ToString("0"));
        spdText.SetText(": " + (PlayerCharacterMovement.Instance.speed * basePlayerAttributesRef.spd).ToString("0.0"));
        dexText.SetText(": " + SetAttackSpeedPlayer.Instance.PlayerAttackSpeed.ToString("0.00"));

    }
}
