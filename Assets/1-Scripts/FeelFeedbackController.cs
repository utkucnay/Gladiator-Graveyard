using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public enum FeelType
{
    ShieldHitFeedback,
    GetHitFeedBack,
    AttackSucceedFeedback,
    DashFeedback,
    ParryFeedback
}
public class FeelFeedbackController : Singleton<FeelFeedbackController>
{
    [System.Serializable]
    public struct Feedbacks
    {
        public FeelType _feelType;
        public MMFeedbacks _feedback;
    }
    public List<Feedbacks> feedbacks = new List<Feedbacks>();
    
    public override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFeedback(FeelType feedbackType)
    {
        if(AnyFeedbackPlayinSon())
        {
            return;
        }
        
        foreach (Feedbacks feedback in feedbacks)
        {
            if (feedback._feelType == feedbackType)
            {
                feedback._feedback.PlayFeedbacks();
            }
        }
    }

    bool AnyFeedbackPlayinSon()
    {
        bool isPlayen = false;

        foreach (Feedbacks feedback in feedbacks)
        {
            if (feedback._feedback.IsPlaying)
            {
                isPlayen = true;
            }
        }

        return isPlayen;
    }
}