using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;

public class NetworkPlayer : NetworkEntity
{
    [Header("Components")]
    public TMP_Text textDisplayName;
    [SerializeField] private Renderer rendererPlayer;
    public static NetworkPlayer localPlayer;

    [Header("Infos Player")]
    [SyncVar(hook = nameof(HandleDisplayNameUpdate))]
    [SerializeField]
    public string displayName = "Unknow Player";
    [SyncVar(hook = nameof(HandleTeamColourUpdate))]
    [SerializeField]
    private Color teamColor = Color.black;
    [SyncVar(hook = nameof(HandleClientScoreUpdated))]
    private int score = 0;
    public event Action<int> ClientOnScoreUpdated;
    public event Action<string> ClientSetName;


    // private void Awake()
    // {
    //     textDisplayName = GetComponentInChildren<TMP_Text>();
    // }

    public int GetScore()
    {
        return score;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        localPlayer = this;
    }

    #region Server

    [Server]
    public void SetScore(int newScore)
    {
        score = newScore;
    }

    [Server]
    public void SetColourTeam(Color newColor)
    {
        teamColor = newColor;
    }

    [Server]
    public void SetDisplayName(string newName)
    {
        displayName = newName;
    }
    #endregion

    #region Client
    private void HandleTeamColourUpdate(Color oldColor, Color newColor)
    {
        rendererPlayer.material.SetColor("_Color", newColor);
    }
    private void HandleDisplayNameUpdate(string oldName, string newName)
    {
        textDisplayName.text = newName;
        ClientSetName?.Invoke(newName);
    }
    private void HandleClientScoreUpdated(int oldScore, int newScore)
    {
        ClientOnScoreUpdated?.Invoke(newScore);
    }

    #endregion
}
