using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ludus : Singleton<Ludus>
{
    public GameObject[] Cards;

    public float LudusShowTime;

    public override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        Ludushide();
    }
    public void LudusShow()
    {
        WaveController.Instance.StartCoroutine(LudusActionTime());
    }

    public void Ludushide()
    {
        gameObject.SetActive(false);
    }

    IEnumerator LudusActionTime()
    {
        yield return new WaitForSeconds(LudusShowTime);
        gameObject.SetActive(true);
    }
}
