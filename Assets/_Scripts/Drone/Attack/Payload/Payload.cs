using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payload : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public void DisconnectWithVelocity(Vector3 disconnectVelocity)
    {
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = disconnectVelocity;
    }
}
