using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class NetworkAI : NetworkEntity
{
    State currentState;
    NavMeshAgent agent;

    [SerializeField]
    private MeshRenderer mRender;
    [SyncVar(hook = nameof(HandleTeamColourUpdate))]
    [SerializeField]
    private Color teamColor = Color.black;

    public string[] TagsOfTargets;
    public Transform[] CheckPoints;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = new Idle(gameObject, agent, TagsOfTargets, CheckPoints);
    }

    [ServerCallback]
    private void Update()
    {
        currentState = currentState.Process();
    }

    [ServerCallback]
    public void OnAggro(Health healthEntity)
    {
        foreach (string t in TagsOfTargets)
        {
            if (t == healthEntity.tag)
            {
                if (agent.isActiveAndEnabled && agent.isStopped)
                    agent.isStopped = false;
                currentState = new Attack(gameObject, agent, TagsOfTargets, CheckPoints, healthEntity);
                break;
            }
        }
    }

    [ServerCallback]
    public void ShotBot()
    {
        GetComponent<Shooter>().BotShot();
    }

    [Server]
    public void SetColourTeam(Color newColor)
    {
        mRender.material.SetColor("_Color", newColor);

        teamColor = newColor;
    }
    private void HandleTeamColourUpdate(Color oldColor, Color newColor)
    {

        mRender.material.SetColor("_Color", newColor);
    }

    private void OnDestroy()
    {
        currentState = null;
    }

}