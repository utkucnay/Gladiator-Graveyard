using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GloryUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textRef;
    int oldGlory;
    private void Update()
    {
        textRef.text = Glory.GetGlory() + "";
    }
}
