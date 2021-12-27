using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HealthDisplay : Display
{
    [SerializeField] private TMP_Text healthTxt = null;
    private Health playerHealth;

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
            playerHealth = PlayerNet.GetComponent<Health>();
            playerHealth.ClientOnHealthUpdated += ClientHandleHealthUpdated;
            healthTxt.text = $"Health: {playerHealth.GetHealth()}";
        }
    }

    private void ClientHandleHealthUpdated(int health)
    {
        healthTxt.text = $"Health: {health}";
    }


    private void OnDestroy()
    {
        playerHealth.ClientOnHealthUpdated -= ClientHandleHealthUpdated;

    }

}
