using System.Collections.Generic;
using UnityEngine;

public class BehaviourPool<T> where T : Behaviour
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Stack<T> _behavioursStack;

    public BehaviourPool(T prefab, int capacity = 8, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        _behavioursStack = GetInitializedBehavioursStack(capacity);
    }

    private Stack<T> GetInitializedBehavioursStack(int capacity)
    {
        Stack<T> newObjectsStack = new Stack<T>(capacity);

        for (int i = 0; i < capacity; i++)
        {
            T newBehaviour = Object.Instantiate(_prefab, _parent);
            newBehaviour.gameObject.SetActive(false);
            newObjectsStack.Push(newBehaviour);
        }

        return newObjectsStack;
    }

    public T Get()
    {
        if (_behavioursStack.Count > 0)
        {
            T obj = _behavioursStack.Pop();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T obj = Object.Instantiate(_prefab, _parent);
            return obj;
        }
    }

    public void Release(T obj) 
    {
        obj.gameObject.SetActive(false);
        _behavioursStack.Push(obj);
    }
}
