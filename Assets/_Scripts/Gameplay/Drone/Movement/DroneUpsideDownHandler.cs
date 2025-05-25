using UnityEngine;

public class DroneUpsideDownHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _minVelocitySqrMagnitude = 0.02f;
    [SerializeField] private float _maxUpsideDownTime = 2f;
    [SerializeField] private float _boxOffset;
    [SerializeField] private float _turnPositionYOffset = 0.025f;
    [SerializeField] private string _mapLayerString = "Map";
    [SerializeField] private Vector3 _boxHalfExtents;

    private int _mapLayerMask;
    private float _upsideDownTimer;
    private Collider[] _mapColliderResult = new Collider[1];

    private void Awake()
    {
        _mapLayerMask = LayerMask.GetMask(_mapLayerString);
    }

    private void FixedUpdate()
    {
        bool isUpsideDown = IsStaticAndUpsideDown();
        if (isUpsideDown)
        {
            TurnDrone();
            _upsideDownTimer = 0f;
        }
    }

    private bool IsStaticAndUpsideDown()
    {
        if (_rigidbody.velocity.sqrMagnitude > _minVelocitySqrMagnitude)
        {
            return false;
        }

        bool isOverlapBoxHittingMap = IsOverlapBoxHittingMap();
        if (isOverlapBoxHittingMap)
        {
            _upsideDownTimer += Time.fixedDeltaTime;
            if (_upsideDownTimer > _maxUpsideDownTime)
            {
                return true;
            }
        }
        else
        {
            _upsideDownTimer = 0f;
        }

        return false;
    }

    private bool IsOverlapBoxHittingMap()
    {
        Vector3 center = transform.position + transform.up * _boxOffset;
        int collidersFound = Physics.OverlapBoxNonAlloc(center, _boxHalfExtents, _mapColliderResult, Quaternion.identity, _mapLayerMask);
        if (collidersFound > 0)
        {
            return true;
        }

        return false;
    }

    private void TurnDrone()
    {
        transform.position += Vector3.up * _turnPositionYOffset;
        transform.rotation = Quaternion.identity;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = transform.position + transform.up * _boxOffset;
        Gizmos.DrawCube(center, _boxHalfExtents * 2f);
    }
}
