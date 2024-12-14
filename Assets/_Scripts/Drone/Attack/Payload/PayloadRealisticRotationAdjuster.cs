using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadRealisticRotationAdjuster : MonoBehaviour
{
    [SerializeField] private Rigidbody _payloadRigidbody;
    [SerializeField, Range(0.1f, 1f)] private float _rotationSpeed = 0.5f;

    private void Update()
    {
        AdjustRotation();
    }

    private void AdjustRotation()
    {
        if (_payloadRigidbody.velocity.sqrMagnitude > 0.1f)
        {
            Vector3 targetDirection = _payloadRigidbody.velocity.normalized;
            transform.up = Vector3.Lerp(transform.up, targetDirection, Time.deltaTime * _rotationSpeed);
        }
    }
}
