using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class ShowCanvas : NetworkBehaviour
{
    [SerializeField]
    private GameObject canvasInfo;


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if (isLocalPlayer)
        {
            canvasInfo.SetActive(false);

            enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (canvasInfo.activeSelf == false && !isLocalPlayer)
        {
            canvasInfo.SetActive(true);
        }

    }
}
