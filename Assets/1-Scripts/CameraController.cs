using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Cinemachine;
using System;

public class CameraController : Singleton<CameraController>
{
    
    [ReorderableList][SerializeField]
    private List<CinemachineVirtualCamera> virtualCameras;

    [ReadOnly]
    public CinemachineVirtualCamera activeCamera;

    public override void Start()
    {
        base.Start();

        SwitchCamera(0);
    }

    public override void RunStarted()
    {

    }

    public override void RunEnded()
    {

    }

    public void SwitchCamera(int cameraIndex)
    {
        foreach(CinemachineVirtualCamera v in virtualCameras)
        {
            v.Priority = 0;
        }

        virtualCameras[cameraIndex].Priority = 1;
        activeCamera = virtualCameras[cameraIndex];
    }
    public void SwitchCamera(int cameraIndex, float timeToSwitch)
    {
        StartCoroutine(SwitchCameraBackCooroutine(cameraIndex, timeToSwitch, 0));
    }

    public void SwitchCamera(int cameraIndex, float timeToSwitch, float timeToSwitchBack)
    {
        StartCoroutine(SwitchCameraBackCooroutine(cameraIndex, timeToSwitch, timeToSwitchBack));
    }

    IEnumerator SwitchCameraBackCooroutine(int cameraIndex, float timeToSwitch, float timeToSwitchBack)
    {
        yield return new WaitForSeconds(timeToSwitch);

        CinemachineVirtualCamera currentCam = null;

        foreach (CinemachineVirtualCamera v in virtualCameras)
        {
            if(v.Priority == 1)
            {
                currentCam = v;
                activeCamera = virtualCameras[cameraIndex];
            }
            v.Priority = 0;
        }

        virtualCameras[cameraIndex].Priority = 1;

        yield return new WaitForSeconds(timeToSwitchBack);

        if(timeToSwitchBack != 0 && currentCam != null)
        {
            foreach (CinemachineVirtualCamera v in virtualCameras)
            {
                v.Priority = 0;
            }

            currentCam.Priority = 1;
        }

        yield break;
    }

  
}
