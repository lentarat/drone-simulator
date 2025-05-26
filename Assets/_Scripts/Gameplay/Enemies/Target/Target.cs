using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public abstract class Target : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private SFXPlayer _deathSFXPlayer;

    public Action OnDeathAction { private get; set; }
    private TargetRouteData _targetRouteData;

    private CancellationTokenSource _cancellationTokenSource = new();

    public void Init(TargetRouteData targetRouteData, AudioController audioController, float targetSpeed)
    {
        _targetRouteData = targetRouteData;
        _deathSFXPlayer.Init(audioController);
        SetNavMeshAgentSpeed(targetSpeed);
    }

    public virtual void Die()
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
        int i = _targetRouteData.WaypointPositionIndex;
        Vector3[] routeWaypointsPositions = _targetRouteData.WaypointsPositions;
        bool goingForward = UnityEngine.Random.Range(0, 2) == 1;
        int offset;
        if (goingForward)
        {
            offset = 1;
        }
        else
        {
            offset = -1;
        }

        while (_cancellationTokenSource.IsCancellationRequested == false) 
        {
            _navMeshAgent.SetDestination(routeWaypointsPositions[i]);

            await UniTask.WaitUntil(() =>
                !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.1f,
                cancellationToken: _cancellationTokenSource.Token);

            int waypointsCount = routeWaypointsPositions.Length;
            if (i == waypointsCount - 1)
            {
                offset = -1;
            }
            else if (i == 0)
            {
                offset = 1;
            }

            i += offset;
            //if (goingForward)
            //{
            //    i = (i + 1) % waypointsCount;
            //}
            //else
            //{
            //    i = (i - 1 + waypointsCount) % waypointsCount;
            //}
        }
    }

    private void SetNavMeshAgentSpeed(float targetSpeed)
    { 
        _navMeshAgent.speed *= targetSpeed;
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }
}
