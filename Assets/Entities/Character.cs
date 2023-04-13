using Entities;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    [SerializeField] private LayerMask CoverLayers;
    [SerializeField] private float Radius = 1;    
    protected override void EntityDestroy()
    {
        print("destroyed");
    }

    protected override void EntityStart()
    {        
    }

    protected override void OnDeath()
    {
        print("Dying");
        gameObject.SetActive(false);
    }

    public bool FindCover(out Vector3 position)
    {
        position = Vector3.zero;
        HashSet<Collider> covers = new HashSet<Collider>(Physics.OverlapSphere(transform.position, Radius, CoverLayers));
        if(covers.Count <= 0)
        {
            return false;
        }        
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (var item in covers)
        { 
            Vector3 directionToTarget = item.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                position = item.ClosestPoint(transform.position);
            }
        }
        return true;
    }
}
