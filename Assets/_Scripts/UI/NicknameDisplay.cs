using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NicknameDisplay : Display
{
    [SerializeField] private TMP_Text nicknameTxt;

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
            nicknameTxt.text = PlayerNet.displayName;
            PlayerNet.ClientSetName += HandleSetName;
        }
    }

    private void HandleSetName(string nick)
    {
        nicknameTxt.text = PlayerNet.displayName;

    }
}
