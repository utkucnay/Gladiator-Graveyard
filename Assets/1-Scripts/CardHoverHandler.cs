using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouseOver = false;

    private Vector3 startScale;

    [SerializeField] private float scaleAmount = 1.15f;
    [SerializeField] private float scaleTime = 0.3f;

    Tween currentEnterTween;
    Tween currentExitTween;

    private void Start()
    {
        startScale = gameObject.GetComponent<RectTransform>().localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioController.Instance.PlayAudio(AudioType.CardHover);
        mouseOver = true;
        if (currentEnterTween != null && currentEnterTween.active)
        {
            currentEnterTween.Kill(false);
        }
        currentEnterTween = gameObject.GetComponent<RectTransform>().DOScale(startScale * scaleAmount, scaleTime).SetEase(Ease.OutCirc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        if(currentExitTween != null && currentExitTween.active)
        {
            currentExitTween.Kill(false);
        }
        currentExitTween = gameObject.GetComponent<RectTransform>().DOScale(startScale * 1f, scaleTime).SetEase(Ease.OutCirc);
    }
}
