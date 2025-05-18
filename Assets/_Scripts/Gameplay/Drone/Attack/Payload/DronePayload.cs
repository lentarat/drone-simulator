using UnityEngine;

public class DronePayload : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SFXPlayer _actionSFXPlayer;

    public void Init(AudioController audioController)
    {
        _actionSFXPlayer.Init(audioController);
    }

    public void DisconnectWithVelocity(Vector3 disconnectVelocity)
    {
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = disconnectVelocity;
    }

    protected void PlaySound()
    {
        _actionSFXPlayer.Play();
    }
}
