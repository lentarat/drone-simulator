using UnityEngine;

public class DronePayload : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SFXPlayer _actionSFXPlayer;

    public void Init(AudioController audioController)
    {
        _actionSFXPlayer.Init(audioController);
    }

    public void DisconnectWithVelocity(Vector3 disconnectVelocity, Vector3 additionalAccelerationVector)
    {
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = disconnectVelocity;
        _rigidbody.AddForce(additionalAccelerationVector, ForceMode.VelocityChange);
    }

    protected void PlaySound()
    {
        _actionSFXPlayer.Play();
    }
}
