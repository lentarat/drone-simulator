using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePayload : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    protected AudioManager AudioManager { get; private set; }

    public void Init(AudioManager audioManager)
    { 
        AudioManager = audioManager;
    }

    public void DisconnectWithVelocity(Vector3 disconnectVelocity)
    {
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = disconnectVelocity;
    }
}
