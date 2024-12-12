using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour, IPayload
{
    [SerializeField] private Rigidbody _rigidbody;

    void IPayload.DisconnectWithVelocity(Vector3 disconnectVelocity)
    {
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = disconnectVelocity;
    }
}
