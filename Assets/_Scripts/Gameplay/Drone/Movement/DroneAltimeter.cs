using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAltimeter : MonoBehaviour
{
    [SerializeField] private string _groundLayerName = "Ground";

    private int _groundLayer;
    private int _maxGroundDetectionDistance = 500;

    public float HeightValue { get; private set; }

    private void Awake()
    {
        _groundLayer = LayerMask.NameToLayer(_groundLayerName);
    }

    private void FixedUpdate()
    {
        HeightValue = GetHeight();        
    }

    private float GetHeight()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, _maxGroundDetectionDistance, _groundLayer))
        { 
            float height = hitInfo.distance;
            return height;
        }

        return HeightValue;
    }
}
