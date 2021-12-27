using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Random = UnityEngine.Random;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class DiepNetworkManager : NetworkManager
{
    public int BlueTeamNumbersOfPlayers;
    public int RedTeamTeamNumbersOfPlayers;
    public int numberOfNpcsPerTeam = 0;
    public int numberOfNpcsPerTeamMax;
    public Team[] teams;

    public override void OnStartServer()
    {
        base.OnStartServer();
        InstanceNpcs();
    }


    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        int numberTeam;
        numberTeam = BlueTeamNumbersOfPlayers > RedTeamTeamNumbersOfPlayers ? 1 : 0;

        if (numberTeam == 0)
        {
            BlueTeamNumbersOfPlayers++;
        }
        else
        {
            RedTeamTeamNumbersOfPlayers++;
        }

        Transform startPos = teams[numberTeam].spawnPoints[Random.Range(0, teams[numberTeam].spawnPoints.Length)];

        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";

        player.tag = teams[numberTeam].name;

        NetworkServer.AddPlayerForConnection(conn, player);


        NetworkPlayer playerNet = conn.identity.GetComponent<NetworkPlayer>();

        playerNet.SetDisplayName($"Tester {numPlayers}");

        playerNet.GetComponent<Shooter>().EquipGun();

        playerNet.SetColourTeam(teams[numberTeam].colour);

    }

    private void InstanceNpcs()
    {
        foreach (Team t in teams)
        {
            if (numberOfNpcsPerTeam == 0)
                for (int i = 0; i < numberOfNpcsPerTeamMax; i++)
                {
                    Transform startPos = t.spawnPoints[Random.Range(0, t.spawnPoints.Length)];

                    GameObject npc = startPos != null
                        ? Instantiate(t.botPrefab, startPos.position, startPos.rotation)
                        : Instantiate(t.botPrefab);

                    npc.tag = t.name;


                    NetworkAI npcAI = npc.GetComponent<NetworkAI>();

                    npcAI.TagsOfTargets = t.tagOfEnemy;

                    npcAI.CheckPoints = t.botsPath;

                    NetworkServer.Spawn(npc);
                    npcAI.SetColourTeam(t.colour);


                }
        }
        numberOfNpcsPerTeam = numberOfNpcsPerTeamMax;
    }
}
