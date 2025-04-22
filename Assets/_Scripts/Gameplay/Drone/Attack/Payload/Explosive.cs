using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class Explosive : DronePayload
{
    //[SerializeField] private float _damage;
    [SerializeField] private float _maxDistanceDealingDamage;
    [SerializeField] private ParticleSystem _explosionVFX;
    [SerializeField] private LayerMask _damageableLayerMask;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        FindDamageables();
        ParticleSystem explosionVFX = Instantiate(_explosionVFX, transform.position, _explosionVFX.transform.rotation);
        Destroy(gameObject);
    }

    private Collider[] FindDamageables()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _maxDistanceDealingDamage, _damageableLayerMask);
        if (colliders != null)
        {
            Debug.Log(colliders.Length);
            foreach (Collider collider in colliders)
            {
                collider.GetComponent<IDamageable>()?.ApplyDamage();
            }
        }
        return colliders;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _maxDistanceDealingDamage);
    }
}
