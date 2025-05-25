using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using Zenject;

public class DronePayloadReleaseSystem : MonoBehaviour
{
    [SerializeField] private DroneMovementSystem _droneMovementSystem;
    [SerializeField] private DronePayload _payloadPrefab;
    [SerializeField] private SFXPlayer _releaseSFXPlayer;
    [SerializeField] private int _nextPayloadSpawnIntervalMS;
    [SerializeField] private float _payloadReleaseAdditionalAccelerationValue;
    [SerializeField] private Transform _payloadPlaceTransform;

    private CancellationToken _token;
    private DronePayload _payload;
    private AudioController _audioController;
    private IPayloadReleaseInvoker _payloadReleaseInvoker;

    [Inject]
    private void Construct(IPayloadReleaseInvoker payloadReleaseInvoker, AudioController audioController)
    {
        _payloadReleaseInvoker = payloadReleaseInvoker;
        _audioController = audioController;
    }

    private void Awake()
    {
        _payloadReleaseInvoker.OnReleaseCalled += HandleBombReleaseCalled;

        _releaseSFXPlayer.Init(_audioController);

        _token = this.GetCancellationTokenOnDestroy();
        DelayedSpawnPayloadAsync().Forget();
    }

    private void HandleBombReleaseCalled()
    {
        Debug.Log(_payload);
        if (_payload == null)
            return;

        DropPayload();
        PlayReleaseSound();
        DelayedSpawnPayloadAsync().Forget();
    }

    private void DropPayload()
    {
        Vector3 payloadVelocityAfterDisconnetion = _droneMovementSystem.Rigidbody.velocity;
        Vector3 droneRigidbodyDown = _droneMovementSystem.Rigidbody.rotation * Vector3.down;
        Vector3 additionalVelocityChangeVector = droneRigidbodyDown * _payloadReleaseAdditionalAccelerationValue;
        _payload.DisconnectWithVelocity(payloadVelocityAfterDisconnetion, additionalVelocityChangeVector);
        _payload = null;
    }

    private void PlayReleaseSound()
    {
        _releaseSFXPlayer.Play();
    }

    private async UniTask DelayedSpawnPayloadAsync()
    {
        try
        {
            await UniTask.Delay(_nextPayloadSpawnIntervalMS, cancellationToken: _token);
            await UniTask.WaitForFixedUpdate();

            _payload = Instantiate(_payloadPrefab, _payloadPlaceTransform);
            _payload.Init(_audioController);
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnDisable()
    {
        _payloadReleaseInvoker.OnReleaseCalled -= HandleBombReleaseCalled;
    }
}
