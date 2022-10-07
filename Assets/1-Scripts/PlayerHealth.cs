using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : CharacterHealth
{
    [SerializeField] private int restPercentage;
    [SerializeField] private Color color;
    [SerializeField] private SpriteRenderer spriteMaterialRef;
    [SerializeField] private float pushMultiplier = 0.4f;
    [SerializeField] private float farBlockAngle = 80;
    [SerializeField] private float shortBlockAngle = 100;
    [SerializeField] private float range = 10;

    public BasePlayerAttributes playerAttributes;

    private CharacterRotator2D characterRotRef;
    private AnimHandle animHandleRef;
    public UnityEvent<HitInfo> blockEvent;
    public override void ReceiveDamage(HitInfo hitInfo)
    {
        if (PlayerCharacterCombat.Instance.combatState == PlayerCombatState.CoverShield)
        {
            float defenceAngle = Vector2.Angle(characterRotRef.currentDirectionVector, -hitInfo.hitDirection);
            bool inShortRange = Vector3.Distance(gameObject.transform.position, hitInfo.attackWeaponPosition) < range;
            if (defenceAngle > (inShortRange ? shortBlockAngle : farBlockAngle))
            {
                ParticleEffectController.Instance.OnHitColorChange(spriteMaterialRef.material, 0.25f, 1f, color, this);
                PlayBloodParticle(hitInfo);

                FeelFeedbackController.Instance.PlayFeedback(FeelType.GetHitFeedBack);
                base.ReceiveDamage(hitInfo);
                PlayerCharacterMovement.Instance.PushSelf(hitInfo);
                AudioController.Instance.PlayAudio(AudioType.FleshHit, 1, 1.4f);

            }
            else if (!PlayerStamina.Instance.ConsumeStamina(hitInfo.staminaConsumeOnShieldHit * PlayerStamina.Instance.reduceStaminaMultipler))
            {
                ParticleEffectController.Instance.OnHitColorChange(spriteMaterialRef.material, 0.25f, 1f, color, this);
                PlayBloodParticle(hitInfo);

                PlayerCharacterCombat.Instance.StopSpecial();
                base.ReceiveDamage(hitInfo);
                PlayerCharacterMovement.Instance.PushSelf(hitInfo);
                
                ParticleSystem p = Instantiate(Storage.Instance.shieldHitParticle, transform.position,
                    quaternion.identity);
                p.Play();
                FeelFeedbackController.Instance.PlayFeedback(FeelType.ShieldHitFeedback);
                AudioController.Instance.PlayAudio(AudioType.FleshHit, 1, 1.4f);

            }
            else
            {
                AudioController.Instance.PlayAudio(AudioType.ShieldHit,1 + (1 - PlayerStamina.Instance.GetCurrentStamina()/PlayerStamina.Instance.GetMaxStamina() * 0.8f));

                if (blockEvent !=null)
                {
                    blockEvent.Invoke(hitInfo);
                }
                ParticleSystem p = Instantiate(Storage.Instance.shieldHitParticle, transform.position,
                    quaternion.identity);
                p.Play();
                FeelFeedbackController.Instance.PlayFeedback(FeelType.ShieldHitFeedback);
                PlayerCharacterMovement.Instance.PushSelf(hitInfo, pushMultiplier);
                if(hitInfo.hitType == HitType.Arrow)
                {
                    ArrowOnShieldActivator.Instance.IncreaseStuckArrows();
                }
            }
        }
        else
        {
            PlayBloodParticle(hitInfo);
            ParticleEffectController.Instance.OnHitColorChange(spriteMaterialRef.material, 0.25f, 1f, color, this);
            FeelFeedbackController.Instance.PlayFeedback(FeelType.GetHitFeedBack);
            base.ReceiveDamage(hitInfo);
            PlayerCharacterMovement.Instance.PushSelf(hitInfo);
            AudioController.Instance.PlayAudio(AudioType.FleshHit, 1, 1.4f);

        }

        if (currentHealth <= 0)
        {
            KillSelf();
        }
    }

    private void PlayBloodParticle(HitInfo hitInfo)
    {
        Vector3 bloodDir = (hitInfo.attackWeaponPosition - gameObject.transform.position).normalized;
        bloodDir = Quaternion.AngleAxis(UnityEngine.Random.Range(-55, 55), Vector3.forward) * bloodDir;
        ParticleEffectController.Instance.PlayBloodParticle(gameObject.transform.position, -bloodDir * 25);
    }

    public void KillSelf()
    {
        GameController.Instance.InvokePlayerDied();
    }

    public override void Start()
    {
        base.Start();
        characterRotRef = gameObject.GetComponent<CharacterRotator2D>();
        animHandleRef = gameObject.GetComponent<AnimHandle>();
        blockEvent = new UnityEvent<HitInfo>();
    }

    void Update()
    {
        PlayerHealthUI.Instance.SetSliderValue(currentHealth / maxHealth);
    }

    public void IncreaseHealth(int number)
    {
        currentHealth += number;
        maxHealth += number;
    }

    public void SetHealthOnData()
    {
        currentHealth = playerAttributes.vit;
        maxHealth = playerAttributes.vit;
    }

    public void RestHealth()
    {
        currentHealth += maxHealth * restPercentage / 100;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}