using UnityEngine;

[RequireComponent(typeof(SphereCollider))] // aggro area trigger
public class AggroArea : MonoBehaviour
{
    public NetworkAI owner;

    // same as OnTriggerStay
    void OnTriggerEnter(Collider co)
    {
        Health entity = co.GetComponent<Health>();
        if (entity) owner.OnAggro(entity);
    }

    void OnTriggerStay(Collider co)
    {
        Health entity = co.GetComponent<Health>();
        if (entity) owner.OnAggro(entity);
    }
}
