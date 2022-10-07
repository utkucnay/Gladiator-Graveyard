using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectController : Singleton<ParticleEffectController>
{
    public GameObject bloodParticle;
    public GameObject stunParticle;
    public GameObject bloodParticleDeath;

    public override void Start()
    {
        base.Start();
    }

    public override void RunEnded()
    {
        base.RunEnded();
    }

    public override void RunStarted()
    {
        base.RunStarted();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBloodParticleDeath(Transform parent, Vector3 lookAtDir)
    {
        GameObject particle = Instantiate(bloodParticleDeath);
        particle.transform.parent = parent;
        particle.transform.localPosition = new Vector3(0,-0.5f,0);
        particle.transform.LookAt(lookAtDir);
        // Destroy(particle, particle.GetComponent<ParticleSystem>().startLifetime*5);
    }

    public void PlayBloodParticle(Vector3 pos, Vector3 lookAtDir)
    {
        GameObject particle = Instantiate(bloodParticle, pos, Quaternion.EulerAngles(0, 90, 0), WaveController.Instance.trash.transform);
        particle.transform.LookAt(lookAtDir);
       // Destroy(particle, particle.GetComponent<ParticleSystem>().startLifetime*5);
    }
    public void PlayStunParticle(Transform parent,Vector3 pos)
    {
        GameObject particle = Instantiate(stunParticle, pos, Quaternion.EulerAngles(75, 0, 0), parent);
        particle.transform.rotation = Quaternion.Euler(75, 0, 0);
        particle.transform.localScale = Vector3.one * 0.8f;
        Destroy(particle, particle.GetComponent<ParticleSystem>().startLifetime*3);
    }

    private void LookAtDir(GameObject objectToChangeRot, Vector3 dirToLook)
    {
        var angle = Mathf.Atan2(dirToLook.y, dirToLook.x) * Mathf.Rad2Deg;
        Quaternion rotToTurn = Quaternion.AngleAxis(angle, Vector3.forward);
        objectToChangeRot.transform.rotation = rotToTurn;

        objectToChangeRot.transform.Rotate(Vector3.right * 90, Space.Self);
        objectToChangeRot.transform.Rotate(Vector3.forward * 90, Space.Self);

    }

    public void ShieldAnimReciveDamage(Coroutine PreCharAnimReciveDamageCorRef, Material material, float ReciveDmageAndDashTime, Color color)
    {
        if (PreCharAnimReciveDamageCorRef != null)
        {
            StopCoroutine(PreCharAnimReciveDamageCorRef);
        }
        PreCharAnimReciveDamageCorRef = StartCoroutine(ShieldAnimReciveDamage(material, PreCharAnimReciveDamageCorRef, ReciveDmageAndDashTime, color));
    }

    public void CharAnimReciveDamage(Coroutine PreCharAnimReciveDamageCorRef, Material material, float reciveDmageAndDashTime, Color color, CharacterHealth Health)
    {
        if (PreCharAnimReciveDamageCorRef != null)
        {
            StopCoroutine(PreCharAnimReciveDamageCorRef);
        }
        PreCharAnimReciveDamageCorRef = StartCoroutine(CharAnimReciveDamageCor(material, PreCharAnimReciveDamageCorRef, reciveDmageAndDashTime, color, Health));
    }
    IEnumerator<WaitForSeconds> CharAnimReciveDamageCor(Material material, Coroutine PreCharAnimReciveDamageCorRef, float reciveDmageAndDashTime, Color color, CharacterHealth Health)
    {
        material.DOColor(color, 0.1f);
        yield return new WaitForSeconds(reciveDmageAndDashTime * 1.1f);
        Health.defaultColor = Color.white;
        material.DOColor(Color.white, 0.1f);
        PreCharAnimReciveDamageCorRef = null;
        yield break;
    }

    public void OnHitColorChange(Material material, float receiveDamageColorTime, float colorMultiplier ,Color color, CharacterHealth Health)
    {
        Color toColor = color * colorMultiplier;
        toColor = new Color(toColor.r, toColor.b, toColor.g, 1);

        Sequence colorSeq = DOTween.Sequence();
        colorSeq.Append(material.DOColor(toColor, receiveDamageColorTime));
        colorSeq.Append(material.DOColor(Health.defaultColor, receiveDamageColorTime * 0.05f));
    }

    IEnumerator<WaitForSeconds> ShieldAnimReciveDamage(Material material, Coroutine PreCharAnimReciveDamageCorRef, float ReciveDmageAndDashTime, Color color)
    {
        
        material.DOColor(color, Mathf.Clamp(ReciveDmageAndDashTime * 2 / 3, 0, 0.1f));
        yield return new WaitForSeconds(ReciveDmageAndDashTime * 0.8f);
        material.DOColor(Color.white, ReciveDmageAndDashTime * 0.2f);
        yield return new WaitForSeconds(ReciveDmageAndDashTime / 3);
        PreCharAnimReciveDamageCorRef = null;
        yield break;
    }

    public IEnumerator SqueezeEffect(Transform transform, float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = transform.localScale;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }
    }

    public IEnumerator ArrowReleaseSqueeze(Transform transform, float xSqueeze, float ySqueeze, float seconds)
    {
        yield return new WaitForSeconds(0.2f);

        Vector3 originalSize = transform.localScale;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        transform.DOScale(newSize, seconds / 5).SetEase(Ease.InCubic);

       /* while (t <= 1.0)
        {
            t += Time.deltaTime / seconds * 5;
            transform.localScale = Vector3.Lerp(originalSize, newSize, t);

            yield return null;
        }
        t = 0f;*/
        yield return new WaitForSeconds(seconds);
        //transform.localScale = originalSize;
        transform.DOScale(originalSize, seconds / 55).SetEase(Ease.OutCubic);

        /*while (t <= 1.0)
        {
            t += Time.deltaTime / seconds *150;
            transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }*/
    }
}
