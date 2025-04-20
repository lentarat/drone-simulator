using UnityEngine;

[CreateAssetMenu(fileName = "DroneProperties", menuName = "ScriptableObjects/DroneProperties")]
public class DronePropertiesHolderSO : ScriptableObject
{
    [SerializeField] private string _name = "DefaultDroneName";
    public string Name => _name;
    [SerializeField] private float _speed = 1f;
    public float Speed => _speed;
}




