using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadRealisticRotationAdjuster : MonoBehaviour
{
    [SerializeField] private Rigidbody _payloadRigidbody;
    [SerializeField] private float _rotationSpeed;

    private void FixedUpdate()
    {
        AdjustRotation();
    }

    private void AdjustRotation()
    {
        if (_payloadRigidbody.velocity.sqrMagnitude > 0.1f)
        {
            Vector3 targetDirection = _payloadRigidbody.velocity.normalized;
            transform.up = Vector3.Slerp(transform.up, targetDirection, Time.fixedDeltaTime * _rotationSpeed);
        }
    }
}
