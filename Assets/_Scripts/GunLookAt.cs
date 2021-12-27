using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GunLookAt : NetworkBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Transform gunTransform;

    Vector3 mousePos;

    #region Client

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (!hasAuthority) return;
        mousePos = GetMousePos();

    }
    [ClientCallback]
    private void FixedUpdate()
    {
        if (!hasAuthority) return;

        CmdGunLookUpdate(mousePos);
    }
    private Vector3 GetMousePos()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    #endregion

    #region Server
    [Command]
    private void CmdGunLookUpdate(Vector3 mousePosition)
    {
        RpcGunLookUpdate(mousePosition);
    }

    [ClientRpc]
    void RpcGunLookUpdate(Vector3 mousePosition)
    {
        mousePosition = new Vector3(mousePosition.x, 0.5f, mousePosition.z);
        gunTransform.LookAt(mousePosition);
    }

    #endregion

}
