using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    int currentIndex = -1;

    public Patrol(GameObject npc, NavMeshAgent nvAgent, string[] tagsOfTargets, Transform[] checkPoints)
            : base(npc, nvAgent, tagsOfTargets, checkPoints)
    {
        Name = STATE.PATROL;
        this.nvAgent.speed = 2.5f;
        this.nvAgent.isStopped = false;
    }
    public override void Enter()
    {
        currentIndex = -1;
        base.Enter();
    }
    public override void Update()
    {
        if (nvAgent.remainingDistance < 1)
        {
            if (currentIndex >= checkPoints.Length - 1)
                currentIndex = 0;
            else
                currentIndex++;

            nvAgent.SetDestination(checkPoints[currentIndex].position);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}