using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System.Threading;

public abstract class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float _hp;

    private int _waypointPositionIndex;
    private Vector3[] _routeWaypointsPositions;
    private CancellationTokenSource _cancellationTokenSource = new();

    public void Init(Vector3[] routeWaypointsPositions, int waypointPositionIndex)
    {
        _routeWaypointsPositions = routeWaypointsPositions;
        _waypointPositionIndex = waypointPositionIndex;
    }

    protected virtual void Die()
    {
        Debug.Log("Target died");
        Destroy(gameObject);
    }

    private void Start()
    {
        StartRoute().Forget();
    }

    private async UniTask StartRoute()
    {
        int i = _waypointPositionIndex;
        bool goingForward = Random.Range(0, 2) == 1;

        while (_cancellationTokenSource.IsCancellationRequested == false) 
        {
            _navMeshAgent.SetDestination(_routeWaypointsPositions[i]);

            await UniTask.WaitUntil(() =>
                !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.1f,
                cancellationToken: _cancellationTokenSource.Token);

            int waypointsCount = _routeWaypointsPositions.Length;
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
