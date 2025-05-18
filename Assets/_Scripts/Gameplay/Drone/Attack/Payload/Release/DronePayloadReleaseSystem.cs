using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public class DronePayloadReleaseSystem : MonoBehaviour
{
    [SerializeField] private DroneMovementSystem _droneMovementSystem;
    [SerializeField] private DronePayload _payloadPrefab;
    [SerializeField] private SFXPlayer _sfxPlayer;
    [SerializeField] private int _nextPayloadSpawnIntervalMS;

    private CancellationToken _token;
    private DronePayload _payload;
    private AudioController _audioController;
    private IPayloadReleaseInvoker _payloadReleasable;

    [Inject]
    private void Construct(IPayloadReleaseInvoker payload, AudioController audioController)
    {
        _payloadReleasable = payload;
        _audioController = audioController;
    }

    private void Awake()
    {
        _payloadReleasable.OnReleaseCalled += HandleBombReleaseCalled;

        _token = this.GetCancellationTokenOnDestroy();
        DelayedSpawnPayloadAsync().Forget();
    }

    private void HandleBombReleaseCalled()
    {
        Debug.Log(_payload);
        if (_payload == null)
            return;

        DropPayload();
        DelayedSpawnPayloadAsync().Forget();
    }

    private void DropPayload()
    {
        Vector3 payloadVelocityAfterDisconnetion = _droneMovementSystem.Velocity;
        _payload.DisconnectWithVelocity(payloadVelocityAfterDisconnetion);
        _payload = null;
    }

    private async UniTask DelayedSpawnPayloadAsync()
    {
        try
        {
            await UniTask.Delay(_nextPayloadSpawnIntervalMS, cancellationToken: _token);
            _payload = Instantiate(_payloadPrefab, transform);
            _payload.Init(_audioController);
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
