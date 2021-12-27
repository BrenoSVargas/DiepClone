using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    public Idle(GameObject npc, NavMeshAgent nvAgent, string[] tagsOfTargets, Transform[] checkPoints)
            : base(npc, nvAgent, tagsOfTargets, checkPoints)
    {
        Name = STATE.IDLE;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (Random.Range(0, 100) < 15)
        {
            nextState = new Patrol(npc, nvAgent, tagsOfTargets, checkPoints);
            stage = EVENT.EXIT;
        }

    }

}