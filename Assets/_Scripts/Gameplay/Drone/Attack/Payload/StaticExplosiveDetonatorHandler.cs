using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaticExplosiveDetonatorHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody _droneRigidbody;
    [SerializeField] private Collider _detonatorCollider;
    [SerializeField] private Explosive _droneExplosive;
    [SerializeField] private float _minSqrMagnituteToDetonate;
    [SerializeField] private float _collisionCheckInterval;

    private float _lastTimeCollisionCheched;

    private void OnCollisionEnter(Collision collision)
    {
        bool isSqrVelocityHighEnough = IsSqrVelocityHighEnough(collision.relativeVelocity.sqrMagnitude);
        if (isSqrVelocityHighEnough == false)
        {
            return;
        }

        bool isCollisionWithDetonator = IsCollisionWithDetonator(collision);
        if (isCollisionWithDetonator)
        {
            Detonate();
        }
    }

    private bool IsCollisionWithDetonator(Collision collision)
    {
        if (Time.time > _lastTimeCollisionCheched + _collisionCheckInterval)
        {
            _lastTimeCollisionCheched = Time.time;
        }
        else
        {
            return false;
        }

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.thisCollider == _detonatorCollider)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsSqrVelocityHighEnough(float droneSqrVelocity)
    {
        if (droneSqrVelocity > _minSqrMagnituteToDetonate)
        {
            return true;
        }

        return false;
    }

    private void Detonate()
    {
        _droneExplosive.Explode(false);
    }
}
