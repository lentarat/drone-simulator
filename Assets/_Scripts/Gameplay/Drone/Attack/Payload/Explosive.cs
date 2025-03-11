using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class Explosive : DronePayload
{
    [SerializeField] private ParticleSystem _explosionVFX;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        ParticleSystem explosionVFX = Instantiate(_explosionVFX, transform.position, _explosionVFX.transform.rotation);
        Destroy(gameObject);
    }
}
