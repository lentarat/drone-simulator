using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetFactory
{
    public Target Create<T>(Vector3 position) where T : Target;
}
