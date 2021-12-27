using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, ATTACK
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE Name;
    protected EVENT stage;
    protected GameObject npc;
    protected NavMeshAgent nvAgent;
    protected string[] tagsOfTargets;
    protected Transform[] checkPoints;
    protected State nextState;


    protected float rangeAttack = 9f;

    public State(GameObject npc, NavMeshAgent nvAgent, string[] tagsOfTargets, Transform[] checkPoints)
    {
        this.npc = npc;
        this.nvAgent = nvAgent;
        this.tagsOfTargets = tagsOfTargets;
        this.checkPoints = checkPoints;
        stage = EVENT.ENTER;
    }
    public virtual void Enter() => stage = EVENT.UPDATE;
    public virtual void Update() => stage = EVENT.UPDATE;
    public virtual void Exit() => stage = EVENT.EXIT;

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
}
