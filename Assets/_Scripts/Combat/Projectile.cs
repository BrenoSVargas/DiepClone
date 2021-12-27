using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody rb = null;
    [SerializeField]
    private float launchForce = 15f;
    [SerializeField]
    private float timeToDestroy = 3f;
    private NetworkEntity shooter = null;

    private void Start()
    {
        rb.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), timeToDestroy);
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    [ServerCallback]
    void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.layer == 0)
            NetworkServer.Destroy(gameObject);
    }
    [ServerCallback]
    public void SetShooter(NetworkEntity player)
    {
        shooter = player;
    }
    [ServerCallback]
    public NetworkEntity GetShooter()
    {
        return shooter;
    }
}
