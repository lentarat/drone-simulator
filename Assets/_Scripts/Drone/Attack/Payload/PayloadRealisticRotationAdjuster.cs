using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadRealisticRotationAdjuster : MonoBehaviour
{
    [SerializeField] private Rigidbody _payloadRigidbody;

    private void Update()
    {
        AdjustRotation();
    }

    private void AdjustRotation()
    {
        if (_payloadRigidbody.velocity.sqrMagnitude > 0.1f)
        {
            transform.up = _payloadRigidbody.velocity;
        }
    }
}
