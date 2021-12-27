using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Display : MonoBehaviour
{
    [HideInInspector]
    public NetworkPlayer PlayerNet;

    // Update is called once per frame
    void Update()
    {
        if (PlayerNet == null)
        {
            GetLocalPlayer();
        }
    }

    public void GetLocalPlayer()
    {
        if (NetworkPlayer.localPlayer)
            PlayerNet = NetworkPlayer.localPlayer;

    }

    public abstract void SetInfos();
}
