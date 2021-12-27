using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    [SyncVar]
    [SerializeField]
    private float speed = 12f;

    #region Server
    [Command]
    private void CmdMove(Vector3 inputValue)
    {
        Vector3 inputNormalize = inputValue.normalized;

        Vector3 pos = transform.position + (inputNormalize * Time.fixedDeltaTime * speed);

        if (!NavMesh.SamplePosition(pos, out NavMeshHit hit, 0.1f, NavMesh.AllAreas)) return;

        transform.position = pos;

    }

    #endregion

    #region Client

    [ClientCallback]
    private void FixedUpdate()
    {
        if (!hasAuthority) return;


        Vector3 inputValue = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (inputValue.sqrMagnitude < 0.01f) return;

        CmdMove(inputValue);
    }

    #endregion
}