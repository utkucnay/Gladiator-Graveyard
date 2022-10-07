using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

public class LudusAnimation : Singleton<LudusAnimation>
{
    public GameObject MaskCircle;
    public GameObject Square;
    Vector3 maxLocalScale;
    public Ease scaleAnimationEaseExit;
    public Ease scaleAnimationEaseEnter;

    public UnityEvent enterLudusEventFinishEvent;
    public UnityEvent exitLudusEventFinishEvent;

    public GameObject arenaMasks;

    private void Awake()
    {
        maxLocalScale = new Vector3(117, 117, 117); 

    }

    private void Update()
    {
        MaskCircle.transform.position = PlayerCharacterMovement.Instance.transform.position;
    }
    public void EnterLudusEvent() 
    {
        MaskCircle.GetComponent<SpriteMask>().enabled = true;
        Square.SetActive(true);
        arenaMasks.SetActive(false);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(MaskCircle.transform.DOScale(Vector3.zero,Ludus.Instance.LudusShowTime).SetEase(scaleAnimationEaseEnter));
        sequence.AppendCallback(() => {
            enterLudusEventFinishEvent?.Invoke();
        });
    }

    public void ExitLudusEvent()
    {
        arenaMasks.SetActive(false);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(MaskCircle.transform.DOScale(maxLocalScale, Ludus.Instance.LudusShowTime).SetEase(scaleAnimationEaseExit));
        sequence.AppendCallback(() => { MaskCircle.GetComponent<SpriteMask>().enabled = false; Square.SetActive(false); arenaMasks.SetActive(true); exitLudusEventFinishEvent?.Invoke(); });
    }
}
