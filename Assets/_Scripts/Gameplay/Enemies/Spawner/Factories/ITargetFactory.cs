using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetFactory<T> where T : Target
{
    public T Create(
        Vector3 position,
        Quaternion rotation,
        Transform parent);
}
