using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GunEntity : NetworkBehaviour
{
    [SerializeField]
    private GunItem gun;

    private float timeToDestroy = 12f;


    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), timeToDestroy);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        Shooter player = other.GetComponent<Shooter>();
        if (player)
        {
            //GunSerializer.WriteGun(gun);
            //NetworkReader reader = new NetworkReader(GunSerializer.writerData);
            player.EquipGun(gun);

            DestroyObjServer();
        }
    }

    [ServerCallback]
    private void DestroyObjServer()
    {
        NetworkServer.Destroy(gameObject);
    }


    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
