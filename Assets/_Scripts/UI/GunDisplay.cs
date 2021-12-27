using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunDisplay : Display
{
    Shooter player;
    [SerializeField] private TMP_Text ammoTxt = null;
    [SerializeField] private TMP_Text nameGunTxt = null;


    private void Update()
    {
        if (PlayerNet == null)
        {
            GetLocalPlayer();

            SetInfos();
        }
    }
    public override void SetInfos()
    {
        if (PlayerNet != null)
        {
            player = PlayerNet.GetComponent<Shooter>();
            ammoTxt.text = player.GetAmmo().ToString();
            nameGunTxt.text = player.EquippedGun.GunName;

            player.ClientOnAmmoUpdated += ClientHandleAmmoUpdated;
            player.ClientOnNameGunUpdated += ClientHandleNameGunUpdated;
        }
    }

    private void ClientHandleAmmoUpdated(int ammo)
    {
        ammoTxt.text = ammo.ToString();
    }

    private void ClientHandleNameGunUpdated(string nameGun)
    {
        nameGunTxt.text = nameGun;
    }


    private void OnDestroy()
    {
        player.ClientOnAmmoUpdated -= ClientHandleAmmoUpdated;
        player.ClientOnNameGunUpdated -= ClientHandleNameGunUpdated;

    }
}
