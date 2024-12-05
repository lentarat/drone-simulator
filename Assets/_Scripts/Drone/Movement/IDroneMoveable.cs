using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDroneMoveable
{
    public Vector2 GetPitchAndRollInputValue { get; }
    public float GetYawInputValue { get; }
    public float GetThrottleInputValue { get; }
}
