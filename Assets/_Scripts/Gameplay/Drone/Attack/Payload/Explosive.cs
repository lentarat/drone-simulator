using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Explosive : DronePayload
{
    [SerializeField] private ParticleSystem _explosionParticleSystem;
    [SerializeField] private AudioClip _explosionAudioClip;
    [SerializeField] private float _explosionAudioCiipVolume;
    [SerializeField] private float _damage;
    [SerializeField] private float _maxDistanceDealingDamage;
    [SerializeField] private LayerMask _damageableLayerMask;

    private float _damageReductionWithDistance = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        FindDamageables();
        Instantiate(_explosionParticleSystem, transform.position, _explosionParticleSystem.transform.rotation);
        PlaySound();
        Destroy(gameObject);
    }

    private Collider[] FindDamageables()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _maxDistanceDealingDamage, _damageableLayerMask);
        if (colliders != null)
        {
            //Debug.Log(colliders.Length);
            foreach (Collider collider in colliders)
            {
                float damageToApply = GetCalculatedDamage(collider.transform.position);
                if (collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    damageable.ApplyDamage(damageToApply);
                }
            }
        }
        return colliders;
    }

    private float GetCalculatedDamage(Vector3 targetPosition) 
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        Debug.Log("Distance equals: " + distanceToTarget);
        
        if (distanceToTarget < _maxDistanceDealingDamage * 0.5f)
        {
            return _damage;
        }
        else
        {
            float distanceToTargetNormalized = distanceToTarget / _maxDistanceDealingDamage;
            float multiplier = Mathf.Exp((1 - distanceToTargetNormalized * 2f) * _damageReductionWithDistance);
            float calculatedDamage = _damage * multiplier;
            return calculatedDamage;
        }
    }

    private void PlaySound()
    {
        AudioManager.Play(_explosionAudioClip, volume: _explosionAudioCiipVolume);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _maxDistanceDealingDamage);
    }
}
