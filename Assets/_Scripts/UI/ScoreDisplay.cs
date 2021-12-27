using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : Display
{
    [SerializeField] private TMP_Text scoreTxt = null;

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
            ClientHandleScoreUpdated(PlayerNet.GetScore());

            PlayerNet.ClientOnScoreUpdated += ClientHandleScoreUpdated;
        }
    }

    private void ClientHandleScoreUpdated(int score)
    {
        scoreTxt.text = score.ToString();
    }

    private void OnDestroy()
    {
        PlayerNet.ClientOnScoreUpdated -= ClientHandleScoreUpdated;
    }


}
