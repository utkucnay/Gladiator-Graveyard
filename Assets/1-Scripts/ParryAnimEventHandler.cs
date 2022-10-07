using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAnimEventHandler : MonoBehaviour
{
    public void StartParryEvent()
    {
        PlayerCharacterCombat.Instance.StartParryEvent();

    }

    public void EndParryEvent()
    {
        PlayerCharacterCombat.Instance.EndParryEvent();
    }
}
