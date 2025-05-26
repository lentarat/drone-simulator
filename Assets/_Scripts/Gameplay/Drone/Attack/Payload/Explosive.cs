using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Explosive : DronePayload
{
    [SerializeField] private ParticleSystem _explosionParticleSystem;
    [SerializeField] private float _maxDistanceDealingDamage;
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private int _maxTargetsCapturedNearExplosion = 5;

    public event Action OnExploded;

    private Collider[] _targetsNearExplosionColliders;

    public void Explode(bool destroyAfterExplosion = true)
    {
        List<Target> nearbyTargetsList = GetNearbyTargetsList();
        DestroyNearbyTargets(nearbyTargetsList);

        Instantiate(_explosionParticleSystem, transform.position, _explosionParticleSystem.transform.rotation);
        PlaySound();

        if (destroyAfterExplosion)
        {
            Destroy(gameObject);
        }

        OnExploded?.Invoke();
    }

    private List<Target> GetNearbyTargetsList()
    {
        int targetsCount = Physics.OverlapSphereNonAlloc(transform.position, _maxDistanceDealingDamage, _targetsNearExplosionColliders, _targetLayerMask);
        List<Target> nearbyTargetsList = new List<Target>();
        for (int i = 0; i < targetsCount; i++)
        {
            if (_targetsNearExplosionColliders[i].TryGetComponent<Target>(out Target target))
            {
                nearbyTargetsList.Add(target);
            }
        }

        return nearbyTargetsList;
    }

    //private List<Target> GetNearbyTargetsList()
    //{
    //    Collider[] targetsColliders = Physics.OverlapSphere(transform.position, _maxDistanceDealingDamage, _targetLayerMask);
    //    List<Target> nearbyTargetsList = new List<Target>();
    //    foreach (Collider collider in targetsColliders) 
    //    {
    //        if (collider.TryGetComponent<Target>(out Target target))
    //        { 
    //            nearbyTargetsList.Add(target);    
    //        }
    //    }
    //    return nearbyTargetsList;
    //}

    private void DestroyNearbyTargets(List<Target> nearbyTargets)
    {
        foreach (Target target in nearbyTargets)
        {
            target.Die();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        InitializeTargetsCollidersNonAllocArray();
    }

    private void InitializeTargetsCollidersNonAllocArray()
    {
        _targetsNearExplosionColliders = new Collider[_maxTargetsCapturedNearExplosion];
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _maxDistanceDealingDamage);
    }
}
