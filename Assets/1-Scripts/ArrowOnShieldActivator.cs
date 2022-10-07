using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowOnShieldActivator : Singleton<ArrowOnShieldActivator>
{
    private float stuckArrowCount;
    private float time;
    [SerializeField]private int arrowRemoveCooldown;

    [SerializeField] private List<GameObject> FirstArrows;
    [SerializeField] private List<GameObject> SecondArrows;
    [SerializeField] private List<GameObject> ThirdArrows;
    [SerializeField] private List<GameObject> FourthArrows;


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > arrowRemoveCooldown)
        {
            time = 0;
            RemoveStuckArrow();
        }
    }


    public void IncreaseStuckArrows()
    {
        time = 0;
        if(stuckArrowCount < 4)
        {
            stuckArrowCount++;

            switch (stuckArrowCount)
            {
                case (1):
                    foreach(GameObject g in FirstArrows)
                    {
                        g.SetActive(true);
                    }
                    break;
                case (2):
                    foreach (GameObject g in SecondArrows)
                    {
                        g.SetActive(true);
                    }
                    break;
                case (3):
                    foreach (GameObject g in ThirdArrows)
                    {
                        g.SetActive(true);
                    }
                    break;
                case (4):
                    foreach (GameObject g in FourthArrows)
                    {
                        g.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void RemoveStuckArrow()
    {
        if(stuckArrowCount > 0)
        {
            switch (stuckArrowCount)
            {
                case (1):
                    foreach (GameObject g in FirstArrows)
                    {
                        g.SetActive(false);
                    }
                    break;
                case (2):
                    foreach (GameObject g in SecondArrows)
                    {
                        g.SetActive(false);
                    }
                    break;
                case (3):
                    foreach (GameObject g in ThirdArrows)
                    {
                        g.SetActive(false);
                    }
                    break;
                case (4):
                    foreach (GameObject g in FourthArrows)
                    {
                        g.SetActive(false);
                    }
                    break;
                default:
                    break;
            }
            stuckArrowCount--;
        }
    }
}

