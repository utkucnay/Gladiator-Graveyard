using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GloryUIController : MonoBehaviour
{
    public TextMeshProUGUI gloryText;

    void Update()
    {
        gloryText.SetText("Glory: " + Glory.GetGlory().ToString());
    }
}
