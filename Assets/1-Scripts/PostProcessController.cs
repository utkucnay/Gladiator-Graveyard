using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
public class PostProcessController : MonoBehaviour
{
    public PostProcessVolume postProcess;
    private ColorGrading colorGrade;
    private ChromaticAberration chromaticAberration;
    private Vignette vignette;
    public float minSaturation = -40;
    public float maxAbbreviation= 0.5f;
    public float maxVignette = 0.5f;

    private void Start()
    {
        postProcess.profile.TryGetSettings(out colorGrade);
        postProcess.profile.TryGetSettings(out chromaticAberration);
        postProcess.profile.TryGetSettings(out vignette);

    }

    void Update()
    {
        float staminaRate = (PlayerStamina.Instance.GetMaxStamina() - PlayerStamina.Instance.GetCurrentStamina()) / PlayerStamina.Instance.GetMaxStamina();

        if(staminaRate > 0.6f)
        {
            colorGrade.saturation.value = (staminaRate - 0.6f) * 10/4  * minSaturation;
        }
        else
        {
            colorGrade.saturation.value = 0;
        }

        if (staminaRate > 0.6f)
        {
            chromaticAberration.intensity.value = (staminaRate - 0.6f) * 10 / 4 * maxAbbreviation;
        }
        else
        {
            chromaticAberration.intensity.value = 0;
        }

        if (staminaRate > 0.6f)
        {
            vignette.intensity.value = (staminaRate - 0.6f) * 10 / 4 * maxVignette;
        }
        else
        {
            vignette.intensity.value = 0;
        }
    }
}
