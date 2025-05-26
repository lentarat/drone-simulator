using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Zenject;

public class DronePayload : MonoBehaviour
{
    [SerializeField] private Rigidbody _payloadRigidbody;
    [SerializeField] private SFXPlayer _actionSFXPlayer;
    [SerializeField] private bool _isKinematic = true;

    public void Init(AudioController audioController)
    {
        _actionSFXPlayer.Init(audioController);
    }

    public void DisconnectWithVelocity(Vector3 disconnectVelocity, Vector3 additionalAccelerationVector)
    {
        transform.parent = null;
        _payloadRigidbody.isKinematic = false;
        _payloadRigidbody.velocity = disconnectVelocity;
        _payloadRigidbody.AddForce(additionalAccelerationVector, ForceMode.VelocityChange);
    }

    protected void PlaySound()
    {
        _actionSFXPlayer.Play();
    }

    protected virtual void Awake()
    {
        FixUnknownBehaviour();
    }

    private void FixUnknownBehaviour()
    {
        RigidbodyInterpolation cachedRigidbodyInterpolation = _payloadRigidbody.interpolation;
        _payloadRigidbody.interpolation = RigidbodyInterpolation.None;
        _payloadRigidbody.interpolation = cachedRigidbodyInterpolation;
    }
}
