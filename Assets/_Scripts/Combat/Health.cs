using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Health : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleClientHealthUpdated))]
    [SerializeField]
    private int health = 1;
    public int GetHealth()
    { return health; }
    public event Action<int> ClientOnHealthUpdated;
    public event Action<int> ClientOnGameOver;
    public int scoreValue;

    [SyncVar(hook = nameof(HandleClientDeathPlayerUpdated))]
    private int playerDeathScore = -1;

    private void HandleClientHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth);
    }
    private void HandleClientDeathPlayerUpdated(int oldScore, int newScore)
    {
        ClientOnGameOver?.Invoke(newScore);
    }

    [SyncVar]
    private double lastHit;


    #region Server
    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile>() != null)
        {
            if (!other.GetComponent<Projectile>().GetShooter().CompareTag(transform.tag))
            {
                --health;
                if (health == 0)
                {
                    NetworkEntity playerEnemy = other.GetComponent<Projectile>().GetShooter();
                    if (playerEnemy is NetworkPlayer)
                        CheckAndSetScoreEnemy((NetworkPlayer)playerEnemy);

                    NetworkPlayer player;
                    bool isPlayer = TryGetComponent<NetworkPlayer>(out player);
                    if (isPlayer)
                        SetDeathScore(player.connectionToClient, player, isPlayer);

                    DestroyObjServer();
                }
            }
        }
    }

    [ServerCallback]
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Health>())
        {
            if (other.tag != transform.tag && lastHit + 1 < NetworkTime.time)
            {
                --health;

                if (other.tag != "Crate")
                    lastHit = NetworkTime.time;
                else
                {
                    lastHit = NetworkTime.time - 0.5f; ;

                }
            }
            if (health == 0)
            {
                NetworkPlayer playerEnemy = other.GetComponent<NetworkPlayer>();
                CheckAndSetScoreEnemy(playerEnemy);

                NetworkPlayer player;
                bool isPlayer = TryGetComponent<NetworkPlayer>(out player);
                if (isPlayer)
                    SetDeathScore(player.connectionToClient, player, isPlayer);

                else
                    DestroyObjServer();
            }
        }
    }


    [Server]
    private void SetDeathScore(NetworkConnection conn, NetworkPlayer player, bool isPlayer)
    {

        playerDeathScore = player.GetScore();
        Debug.Log(playerDeathScore);
        Invoke(nameof(DestroyObjServer), 0.15f);

    }

    [ServerCallback]
    private void CheckAndSetScoreEnemy(NetworkPlayer playerEnemy)
    {
        if (playerEnemy != null)
        {
            playerEnemy.SetScore(playerEnemy.GetScore() + scoreValue);
        }
    }

    [ServerCallback]
    private void DestroyObjServer()
    {
        NetworkServer.Destroy(gameObject);
    }
    #endregion
}
