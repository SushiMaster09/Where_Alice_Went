using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVCam : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;

    private void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();

        vCam.Follow = PlayerMovement.instance.gameObject.transform;
    }
}
