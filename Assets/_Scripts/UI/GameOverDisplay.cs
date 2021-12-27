using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverDisplay : Display
{

    private GameObject gameOverPanel;
    private TMP_Text scoreTxt;
    Health playerHealth;


    private void Awake()
    {
        gameOverPanel = transform.Find("GameOverPanel").gameObject;
        scoreTxt = gameOverPanel.transform.Find("ScoreTxt").GetComponent<TMP_Text>();

        gameOverPanel.SetActive(false);
    }
    private void Update()
    {
        if (PlayerNet == null)
            SetDisplay();

    }
    private void SetDisplay()
    {
        GetLocalPlayer();
        SetInfos();
    }

    public override void SetInfos()
    {
        if (PlayerNet != null)
        {
            playerHealth = PlayerNet.GetComponent<Health>();

            playerHealth.ClientOnGameOver += ClientHandleGameOver;
        }
    }

    public void ClientHandleGameOver(int score)
    {
        gameOverPanel.SetActive(true);
        scoreTxt.text = $"Score:\n{score}";
    }

    private void OnDestroy()
    {
        PlayerNet.GetComponent<Health>().ClientOnGameOver -= ClientHandleGameOver;

    }
}
