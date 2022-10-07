using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardController : Singleton<CardController>
{
    //common
    [HideInInspector] public Card SharperWeapon;
    [HideInInspector] public Card UnyieldingWill;
    [HideInInspector] public Card BreathingTechniques;
    [HideInInspector] public Card BiggerLungs;
    [HideInInspector] public Card AgileHands;
    [HideInInspector] public Card QuickLegs;

    // murmillo
    [HideInInspector] public Card SpikedShield;
    [HideInInspector] public Card MirrorShield;
    [HideInInspector] public Card TossShield;
    [HideInInspector] public Card FirmGrip;
    [HideInInspector] public Card TransformationOfPower;
    [HideInInspector] public Card RegenarativeParry;
    [HideInInspector] public Card StrongParry;
    [HideInInspector] public Card ArmProtection;
    [HideInInspector] public Card BuildingAnger;

    public Sprite[] commonSprites;
    public Sprite[] specialSprites;

    public List<Card> CommonCards { get; private set; }
    public List<Card> SpecialMurmilloCards { get; private set; }

    public System.Func<int, Card[]> GetRandomCammonCardsbyNumber;
    public System.Func<int, Card[]> GetRandomSpecialCardsbyNumber;

    PlayerHealth playerHealth;

    private int buildingAngerLimit = 0;

    public override void Awake()
    {
        base.Start();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        GetRandomCammonCardsbyNumber = number => GetRandomAllCardsbyNumber(number, CommonCards);
        GetRandomSpecialCardsbyNumber = number => GetRandomAllCardsbyNumber(number, SpecialMurmilloCards);
        CommonCards = new List<Card>();
        SpecialMurmilloCards = new List<Card>();
        CreateCards();
        CardAssort();
    }

    public override void RunStarted()
    {
        base.RunStarted();
        ResetCards();
        CreateCards();
        CardAssort();
    }

    public override void RunEnded()
    {
        base.RunEnded();
        ResetCards();
    }
    void ResetCards()
    {
        CommonCards.Clear();
        SpecialMurmilloCards.Clear();
    }

    public void BuyCard(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.Dexterity:
                AgileHands.InvokeEvent();
                break;
            case CardType.Vitality:
                UnyieldingWill.InvokeEvent();
                break;
            case CardType.Strength:
                SharperWeapon.InvokeEvent();
                break;
            case CardType.Speed:
                QuickLegs.InvokeEvent();
                break;
            case CardType.Stamina:
                BiggerLungs.InvokeEvent();
                break;
            case CardType.SpikyShield:
                SpikedShield.InvokeEvent();
                break;
            case CardType.BuildingAnger:
                BuildingAnger.InvokeEvent();
                break;
            case CardType.FirmGrip:
                FirmGrip.InvokeEvent();
                break;
            case CardType.TransformationOfPower:
                TransformationOfPower.InvokeEvent();
                break;
            case CardType.StrongParry:
                StrongParry.InvokeEvent();
                break;
            default:
                break;
        }
    }

    public CardInfo GetCardInfo(CardType cardType)
    {
        CardInfo cardInfo = new CardInfo();
        switch (cardType)
        {
            case CardType.Dexterity:
                cardInfo.cost = AgileHands.Cost;
                cardInfo.title = AgileHands.Name;
                cardInfo.details = AgileHands.Definition;
                cardInfo.cardImage = AgileHands.Image;
                break;
            case CardType.Vitality:
                cardInfo.cost = UnyieldingWill.Cost;
                cardInfo.title = UnyieldingWill.Name;
                cardInfo.details = UnyieldingWill.Definition;
                cardInfo.cardImage = UnyieldingWill.Image;

                break;
            case CardType.Strength:
                cardInfo.cost = SharperWeapon.Cost;
                cardInfo.title = SharperWeapon.Name;
                cardInfo.details = SharperWeapon.Definition;
                cardInfo.cardImage = SharperWeapon.Image;

                break;
            case CardType.Speed:
                cardInfo.cost = QuickLegs.Cost;
                cardInfo.title = QuickLegs.Name;
                cardInfo.details = QuickLegs.Definition;
                cardInfo.cardImage = QuickLegs.Image;

                break;
            case CardType.Stamina:
                cardInfo.cost = BiggerLungs.Cost;
                cardInfo.title = BiggerLungs.Name;
                cardInfo.details = BiggerLungs.Definition;
                cardInfo.cardImage = BiggerLungs.Image;

                break;
            case CardType.SpikyShield:
                cardInfo.cost = SpikedShield.Cost;
                cardInfo.title = SpikedShield.Name;
                cardInfo.details = SpikedShield.Definition;
                cardInfo.cardImage = SpikedShield.Image;
                break;
            case CardType.FirmGrip:
                cardInfo.cost = FirmGrip.Cost;
                cardInfo.title = FirmGrip.Name;
                cardInfo.details = FirmGrip.Definition;
                cardInfo.cardImage = FirmGrip.Image;
                break;
            case CardType.TransformationOfPower:
                cardInfo.cost = TransformationOfPower.Cost;
                cardInfo.title = TransformationOfPower.Name;
                cardInfo.details = TransformationOfPower.Definition;
                cardInfo.cardImage = TransformationOfPower.Image;
                break;
            case CardType.StrongParry:
                cardInfo.cost = StrongParry.Cost;
                cardInfo.title = StrongParry.Name;
                cardInfo.details = StrongParry.Definition;
                cardInfo.cardImage = StrongParry.Image;
                break;
            case CardType.BuildingAnger:
                cardInfo.cost = BuildingAnger.Cost;
                cardInfo.title = BuildingAnger.Name;
                cardInfo.details = BuildingAnger.Definition;
                cardInfo.cardImage = BuildingAnger.Image;
                break;
            default:
                break;
        }

        return cardInfo;
    }

    void CreateCards()
    {
        AgileHands = new Card(CardType.Dexterity, "Dexterity Training", commonSprites[0], "Increases your dexterity by %5.", () =>
        {
            if (Glory.GetGlory() < AgileHands.Cost)
            {
                return;
            }
            Glory.RemoveGlory(AgileHands.Cost);
            AgileHands.Cost += AgileHands.BaseCost / 3;
            SetAttackSpeedPlayer.Instance.PlayerAttackSpeed += 0.05f;
        

        }, 30);
        SharperWeapon = new Card(CardType.Strength, "Strength Training", commonSprites[1], "Increases your strength by %5.", () =>
        {
            if (Glory.GetGlory() < SharperWeapon.Cost)
            {
                return;
            }
            Glory.RemoveGlory(SharperWeapon.Cost);
            SharperWeapon.Cost += SharperWeapon.BaseCost / 3;
            PlayerCharacterCombat.Instance.DamageMultiplier += 0.05f;
        },30);
        UnyieldingWill = new Card(CardType.Vitality, "Vitality Training", commonSprites[2], "Increases your max vitality by 10 points.", () =>
        {
            if (Glory.GetGlory() < UnyieldingWill.Cost)
            {
                return;
            }
            Glory.RemoveGlory(UnyieldingWill.Cost);
            UnyieldingWill.Cost += UnyieldingWill.BaseCost / 3;
            playerHealth.IncreaseHealth(10);
        },30);
        BreathingTechniques = new Card("Breathing Techniques", commonSprites[0], "Increases your stamina regeneration %20.", () => 
        {
            
        });
        BiggerLungs = new Card(CardType.Stamina, "Stamina Training", commonSprites[3], "Increases max stamina by 10 points.", () => 
        {
            if (Glory.GetGlory() < BiggerLungs.Cost)
            {
                return;
            }
            Glory.RemoveGlory(BiggerLungs.Cost);
            BiggerLungs.Cost += BiggerLungs.BaseCost / 3;
            PlayerStamina.Instance.IncreaseMaxStamina(10);
        },30);

        SpikedShield = new Card(CardType.SpikyShield, "Spiky Shield", specialSprites[0], "Gain the ability to damage(5) enemies while defending against melee attacks.", () =>
        {
            if (Glory.GetGlory() < SpikedShield.Cost)
            {
                return;
            }
            playerHealth.blockEvent.AddListener(hitInfo => 
            {
                if (hitInfo.hitType == HitType.MeleeSword || hitInfo.hitType == HitType.MeleeDefault)
                {
                    Collider2D collider = Physics2D.OverlapCircle(hitInfo.attackWeaponPosition, 0.2f, LayerMask.GetMask("Enemy"));
                    collider.gameObject.GetComponent<EnemyHealth>().ReceiveDamage(new HitInfo(5, 0, 0, Vector3.zero, Vector3.zero, PlayerCombatState.Idle, HitType.MeleeDefault));
                }
            });
            Glory.RemoveGlory(SpikedShield.Cost);
            SpecialMurmilloCards.Remove(SpikedShield);
            LudusStatsUIController.Instance.BuySpecialCard(SpikedShield);
            AudioController.Instance.PlayAudio(AudioType.CardDeal);

        }, 100);
        MirrorShield = new Card("Mirror Shield", specialSprites[0], "Gain the ability to reflect ranged attacks. (This doesn’t apply to aoe damage)", () =>
        {
            SpecialMurmilloCards.Remove(MirrorShield);
            
        });
        TossShield = new Card("Toss Shield", specialSprites[0], "Gain the ability to attack(5) and push enemies with shield. Deals great poise(30) damage.", () =>
        {
            SpecialMurmilloCards.Remove(TossShield);
            
        });
        FirmGrip = new Card(CardType.FirmGrip, "Firm Grip", specialSprites[2], "Consume %30 less stamina while blocking attacks.", () =>
        {
            if (Glory.GetGlory() < FirmGrip.Cost)
            {
                return;
            }
            PlayerStamina.Instance.SetReduceStaminaMultipler(0.7f);
            Glory.RemoveGlory(FirmGrip.Cost);
            SpecialMurmilloCards.Remove(FirmGrip);
            LudusStatsUIController.Instance.BuySpecialCard(FirmGrip);
            AudioController.Instance.PlayAudio(AudioType.CardDeal);


        }, 100);
        TransformationOfPower = new Card(CardType.TransformationOfPower, "Transformation Of Power", specialSprites[4], "After parrying an enemy gain %30 damage increase.", () =>
        {
            if (Glory.GetGlory() < TransformationOfPower.Cost)
            {
                return;
            }
            PlayerCharacterCombat.Instance.parryEvent.AddListener(() => StartCoroutine(CardCor(
                () => PlayerCharacterCombat.Instance.DamageMultiplier += 0.3f, () => PlayerCharacterCombat.Instance.DamageMultiplier -= 0.3f, 1.2f
                )));
            Glory.RemoveGlory(TransformationOfPower.Cost);
            SpecialMurmilloCards.Remove(TransformationOfPower);
            LudusStatsUIController.Instance.BuySpecialCard(TransformationOfPower);
            AudioController.Instance.PlayAudio(AudioType.CardDeal);


        }, 100);
        RegenarativeParry = new Card("Regenarative Parry", specialSprites[0], "After parrying an enemy heal 5 hp.", () =>
        {
            SpecialMurmilloCards.Remove(RegenarativeParry);
        });
        StrongParry = new Card(CardType.StrongParry, "Strong Parry", specialSprites[3], "After parrying an enemy stun the enemies near you for 2 seconds.", () =>
        {
            if (Glory.GetGlory() < StrongParry.Cost)
            {
                return;
            }
            PlayerCharacterCombat.Instance.parryEvent.AddListener(() => 
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(PlayerCharacterMovement.Instance.gameObject.transform.position, 2f, LayerMask.GetMask("Enemy"));
                foreach (var item in colliders)
                {
                    var poise = item.GetComponent<CharacterPoise>();
                    poise.ReducePoise(poise.MaxPoise);
                }
            });
            Glory.RemoveGlory(StrongParry.Cost);
            SpecialMurmilloCards.Remove(StrongParry);
            LudusStatsUIController.Instance.BuySpecialCard(StrongParry);
            AudioController.Instance.PlayAudio(AudioType.CardDeal);


        }, 100);
        ArmProtection = new Card("Arm Protection", specialSprites[0], "Reduce all received damage by 2.", () =>
        {
            SpecialMurmilloCards.Remove(ArmProtection);
        });
        QuickLegs = new Card(CardType.Speed, "Speed Training", commonSprites[4], "Increases your speed by %5.", () =>
        {
            if (Glory.GetGlory() < QuickLegs.Cost)
            {
                return;
            }
            Glory.RemoveGlory(QuickLegs.Cost);
            QuickLegs.Cost += QuickLegs.BaseCost / 3;
            PlayerCharacterMovement.Instance.playerAttributes.spd += 0.05f;
        },30);
        BuildingAnger = new Card(CardType.BuildingAnger, "Building Anger", specialSprites[1], "Blocking an attack gives you %5 bonus damage for a while. (This effect can be stacked 5 times)", () =>
        {
            if (Glory.GetGlory() < BuildingAnger.Cost)
            {
                return;
            }
            playerHealth.blockEvent.AddListener( hitInfo => 
            {
                if (buildingAngerLimit < 5)
                {
                    return;
                }
                StartCoroutine(CardCor(() => { PlayerCharacterCombat.Instance.DamageMultiplier += 0.05f; buildingAngerLimit++; }, 
                    () => { PlayerCharacterCombat.Instance.DamageMultiplier -= 0.05f; buildingAngerLimit--; }, 0.2f));
            });
            Glory.RemoveGlory(BuildingAnger.Cost);
            SpecialMurmilloCards.Remove(BuildingAnger);
            LudusStatsUIController.Instance.BuySpecialCard(BuildingAnger);
            AudioController.Instance.PlayAudio(AudioType.CardDeal);

        }, 100);
    }

    void CardAssort()
    {
        CommonCards.Add(SharperWeapon);
        CommonCards.Add(UnyieldingWill);
        CommonCards.Add(QuickLegs);
        CommonCards.Add(BiggerLungs);
        CommonCards.Add(AgileHands);

        SpecialMurmilloCards.Add(SpikedShield);
        //SpecialMurmilloCards.Add(MirrorShield);
        //SpecialMurmilloCards.Add(TossShield);
        SpecialMurmilloCards.Add(FirmGrip);
        SpecialMurmilloCards.Add(TransformationOfPower);
        //SpecialMurmilloCards.Add(RegenarativeParry);
        SpecialMurmilloCards.Add(StrongParry);
        //SpecialMurmilloCards.Add(ArmProtection);
        //SpecialMurmilloCards.Add(QuickLegs);
        SpecialMurmilloCards.Add(BuildingAnger);
    }

    public Card[] GetRandomAllCardsbyNumber(int number, List<Card> cardList)
    {
        Card[] cards = new Card[number];

        for (int i = 0; i < number; i++)
        {
            cards[i] = cardList[Random.Range(0, cardList.Count)];
        }

        return cards;
    }

     IEnumerator CardCor(UnityAction action, UnityAction action2,float delay)
    {
        action();
        yield return new WaitForSeconds(delay);
        action2();
    }

    public void ResetProp()
    {
        playerHealth.blockEvent.RemoveAllListeners();
        PlayerCharacterCombat.Instance.DamageMultiplier = 1;
        PlayerCharacterCombat.Instance.parryEvent.RemoveAllListeners();
        PlayerStamina.Instance.SetReduceStaminaMultipler(1f);
    }
}
