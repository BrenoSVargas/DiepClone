using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    Health target;
    public Attack(GameObject npc, NavMeshAgent nvAgent, string[] tagsOfTargets, Transform[] checkPoints, Health target)
            : base(npc, nvAgent, tagsOfTargets, checkPoints)
    {
        Name = STATE.PATROL;
        this.nvAgent.speed = 4.5f;
        this.target = target;
    }

    public override void Enter()
    {
        base.Enter();
        if (target != null)
            nvAgent.SetDestination(target.transform.position);
        else
        {
            nextState = new Patrol(npc, nvAgent, tagsOfTargets, checkPoints);
            stage = EVENT.EXIT;
        }
    }

    public override void Update()
    {
        base.Update();

        if (target == null) return;
        if (Vector3.Distance(target.transform.position, npc.transform.position) < rangeAttack)
        {
            nvAgent.isStopped = true;

            npc.GetComponent<NetworkAI>().ShotBot();
        }
    }
}