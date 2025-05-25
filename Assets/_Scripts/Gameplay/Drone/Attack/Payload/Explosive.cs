using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Explosive : DronePayload
{
    [SerializeField] private ParticleSystem _explosionParticleSystem;
    [SerializeField] private float _maxDistanceDealingDamage;
    [SerializeField] private LayerMask _targetLayerMask;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        List<Target> nearbyTargetsList = GetNearbyTargetsList();
        DestroyNearbyTargets(nearbyTargetsList);

        Instantiate(_explosionParticleSystem, transform.position, _explosionParticleSystem.transform.rotation);
        PlaySound();

        Destroy(gameObject);
    }

    private List<Target> GetNearbyTargetsList()
    {
        Collider[] targetsColliders = Physics.OverlapSphere(transform.position, _maxDistanceDealingDamage, _targetLayerMask);
        List<Target> nearbyTargetsList = new List<Target>();
        foreach (Collider collider in targetsColliders) 
        {
            if (collider.TryGetComponent<Target>(out Target target))
            { 
                nearbyTargetsList.Add(target);    
            }
        }
        return nearbyTargetsList;
    }

    private void DestroyNearbyTargets(List<Target> nearbyTargets)
    {
        foreach (Target target in nearbyTargets)
        {
            target.Die();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _maxDistanceDealingDamage);
    }
}
