using UnityEngine;
using System.Collections.Generic;

public class StealHitBox : MonoBehaviour
{
    private readonly List<Collider> _colliders = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (!_colliders.Contains(other))
    {
        _colliders.Add(other);
    }
    }

    private void OnTriggerExit(Collider other)
    {
        _colliders.Remove(other);
    }


    public Stealable GetStealable()
    {
        Stealable target = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider collider in _colliders)
        {
            if (collider == null)
            continue;

            Stealable stealable = collider.GetComponentInParent<Stealable>();

            if (stealable == null || stealable.IsStolen)
               continue;

            float distance = Vector3.Distance(transform.position, stealable.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                target = stealable;
            }
        }

    return target;
    }


public EnemyBase GetEnemy()
{
    EnemyBase target = null;
    float nearestDistance = float.MaxValue;

    foreach (Collider collider in _colliders)
    {
        if (collider == null)
            continue;

        EnemyBase enemy = collider.GetComponentInParent<EnemyBase>();

        if (enemy == null || enemy.State != EnemyBase.EnemyState.Normal)
            continue;

        float distance = Vector3.Distance(transform.position, enemy.transform.position);

        if (distance < nearestDistance)
        {
            nearestDistance = distance;
            target = enemy;
        }
    }

    return target;
}
}
