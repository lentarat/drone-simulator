using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DroneCollisionSoundPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SFXPlayer _collisionSFXPlayer;
    [SerializeField] private float _maxSoundVolume;
    [SerializeField] private float _minSoundVolume;
    [SerializeField] private float _maxVelocitySqrMagnitude;

    [Inject]
    private void Construct(AudioController audioController)
    {
        _collisionSFXPlayer.Init(audioController);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlaySound();
    }

    private void PlaySound()
    {
        float velocitySqrMagnitude = _rigidbody.velocity.sqrMagnitude;
        velocitySqrMagnitude = Mathf.Clamp(velocitySqrMagnitude, 0, _maxVelocitySqrMagnitude);
        float velocitySqrMagnitudeNormalized = velocitySqrMagnitude / _maxVelocitySqrMagnitude;
        float collisionSoundVolume = Mathf.Lerp(_minSoundVolume, _maxSoundVolume, velocitySqrMagnitudeNormalized);
        _collisionSFXPlayer.Play(collisionSoundVolume);
    }
}
