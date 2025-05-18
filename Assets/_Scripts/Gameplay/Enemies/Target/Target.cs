using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public abstract class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private SFXPlayer _deathSFXPlayer;
    [SerializeField] private float _hp;

    public Action OnDeathAction { private get; set; }
    public TargetRouteData RouteData { private get; set; }

    private CancellationTokenSource _cancellationTokenSource = new();

    public void SetAudioController(AudioController audioController)
    {
        _deathSFXPlayer.Init(audioController);
    }

    protected virtual void Die()
    {
        OnDeathAction?.Invoke();
        PlayDeathSound();
        Destroy(gameObject);
    }

    private void PlayDeathSound()
    {
        _deathSFXPlayer.Play();
    }

    private void Start()
    {
        StartRoute().Forget();
    }

    private async UniTask StartRoute()
    {
        int i = RouteData.WaypointPositionIndex;
        Vector3[] routeWaypointsPositions = RouteData.WaypointsPositions;
        bool goingForward = UnityEngine.Random.Range(0, 2) == 1;

        while (_cancellationTokenSource.IsCancellationRequested == false) 
        {
            _navMeshAgent.SetDestination(routeWaypointsPositions[i]);

            await UniTask.WaitUntil(() =>
                !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.1f,
                cancellationToken: _cancellationTokenSource.Token);

            int waypointsCount = routeWaypointsPositions.Length;
            if (goingForward)
            {
                i = (i + 1) % waypointsCount;
            }
            else
            {
                i = (i - 1 + waypointsCount) % waypointsCount;
            }
        }
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    void IDamageable.ApplyDamage(float damage)
    {
        Debug.Log("Damage applied: " + damage);
        Die();
    }
}
