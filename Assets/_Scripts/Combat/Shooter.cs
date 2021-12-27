using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Shooter : NetworkBehaviour
{
    public GunItem EquippedGun;
    [SerializeField]
    private GameObject gunModel = null;
    [SerializeField]
    private GameObject projectilePrefab = null;
    [SerializeField]
    private Transform originProjectilSpawnPoint = null;

    [SerializeField]
    [SyncVar(hook = nameof(HandleClientNameGunUpdated))]
    private string EquippedGunName = "none";
    public event Action<string> ClientOnNameGunUpdated;

    [SerializeField]
    [SyncVar]
    private float rateShot = 1f;
    [SerializeField]
    [SyncVar(hook = nameof(HandleClientAmmoUpdated))]
    private int ammo = 0;
    public int GetAmmo() { return ammo; }
    public event Action<int> ClientOnAmmoUpdated;


    private double lastShotTime = 0;

    private void Update()
    {
        if (!hasAuthority) return;

        if (!InputShot()) return;

        if (!CanShot()) return;

        lastShotTime = NetworkTime.time;

        CmdShot(ammo);
    }


    private bool CanShot() => NetworkTime.time >= ((1 / rateShot) + lastShotTime);

    private bool InputShot()
    {
        return (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space));
    }

    private void HandleClientAmmoUpdated(int oldAmmo, int newAmmo)
    {
        ClientOnAmmoUpdated?.Invoke(newAmmo);
    }

    private void HandleClientNameGunUpdated(string oldName, string newName)
    {
        ClientOnNameGunUpdated?.Invoke(newName);
    }


    #region Server
    [Command]
    void CmdShot(int ammoInGun)
    {
        //Validate Server
        if (0 < ammoInGun && ammoInGun <= EquippedGun.MaxAmmo)
        {
            Quaternion look = originProjectilSpawnPoint.transform.parent.rotation;
            GameObject projectileInstance = Instantiate(projectilePrefab, originProjectilSpawnPoint.position, look);
            projectileInstance.GetComponent<Projectile>().SetShooter(GetComponent<NetworkEntity>());

            NetworkServer.Spawn(projectileInstance, connectionToClient);

            ammo--;
        }
    }
    [Server]
    public void BotShot()
    {
        if (NetworkTime.time >= ((1 / rateShot) + lastShotTime))
        {
            lastShotTime = NetworkTime.time;

            Quaternion look = originProjectilSpawnPoint.transform.parent.rotation;
            GameObject projectileInstance = Instantiate(projectilePrefab, originProjectilSpawnPoint.position, look);
            projectileInstance.GetComponent<Projectile>().SetShooter(GetComponent<NetworkEntity>());


            NetworkServer.Spawn(projectileInstance);
        }
    }

    [Server]
    public void SetAmmo(int newAmmo)
    {
        ammo = newAmmo;
    }

    [Server]
    public void EquipGun(GunItem equippedGun)
    {
        if (EquippedGun != equippedGun)
        {
            SetGunStats(equippedGun);
        }
        SetAmmo(equippedGun.MaxAmmo);
    }

    [Server]
    public void EquipGun()
    {
        EquipGun(EquippedGun);
    }

    [Server]
    public void SetGunStats(GunItem equippedGun)
    {

        EquippedGun = equippedGun;
        rateShot = equippedGun.RateShot;
        EquippedGunName = equippedGun.GunName;
        SetGunModel(equippedGun.GunColor);
    }

    [ClientRpc]
    public void SetGunModel(Color gunColor)
    {
        gunModel.GetComponent<MeshRenderer>().material.SetColor("_Color", gunColor);

    }

    #endregion
}
