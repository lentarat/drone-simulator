using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public class DronePayloadReleaseSystem : MonoBehaviour
{
    [SerializeField] private DroneMovementSystem _droneMovementSystem;
    [SerializeField] private DronePayload _payloadPrefab;
    [SerializeField] private int _nextPayloadSpawnIntervalMS;

    private IPayloadReleaseInvoker _payloadReleasable;
    private CancellationToken _token;
    private DronePayload _payload;
    private AudioManager _audioManager;

    [Inject]
    private void Construct(IPayloadReleaseInvoker payload, AudioManager audioManager)
    {
        _payloadReleasable = payload;
        _audioManager = audioManager;
    }

    private void Awake()
    {
        _payloadReleasable.OnReleaseCalled += HandleBombReleaseCalled;

        _token = this.GetCancellationTokenOnDestroy();
        DelayedSpawnPayload().Forget();
    }

    private void HandleBombReleaseCalled()
    {
        Debug.Log(_payload);
        if (_payload == null)
            return;

        DropPayload();
        DelayedSpawnPayload().Forget();
    }

    private void DropPayload()
    {
        Vector3 payloadVelocityAfterDisconnetion = _droneMovementSystem.Velocity;
        _payload.DisconnectWithVelocity(payloadVelocityAfterDisconnetion);
        _payload = null;
    }

    private async UniTaskVoid DelayedSpawnPayload()
    {
        try
        {
            await UniTask.Delay(_nextPayloadSpawnIntervalMS, cancellationToken: _token);
            _payload = Instantiate(_payloadPrefab, transform);
            _payload.Init(_audioManager);
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnDisable()
    {
        _payloadReleasable.OnReleaseCalled -= HandleBombReleaseCalled;
    }
}
