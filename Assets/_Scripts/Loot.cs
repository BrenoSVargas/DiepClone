using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Loot : NetworkBehaviour
{
    [SerializeField]
    List<GameObject> gunsDrop;
    [SerializeField]
    [Range(0, 99)]
    private int DropLootChance = 0;

    [ServerCallback]
    public void DropItem()
    {
        if (!DropChance()) return;

        GameObject dropItem =
        Instantiate(gunsDrop[Random.Range(0, gunsDrop.Count)], transform.position, Quaternion.identity);

        NetworkServer.Spawn(dropItem);
    }

    [Server]
    bool DropChance() => Random.Range(0, 100) < DropLootChance;

    [Server]
    private void OnDestroy()
    {
        DropItem();
    }
}
