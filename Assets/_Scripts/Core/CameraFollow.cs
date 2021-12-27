using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class CameraFollow : NetworkBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera = null;

    public override void OnStartAuthority()
    {
        virtualCamera.gameObject.SetActive(true);
        enabled = true;
    }
}
