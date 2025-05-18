using System.Collections;
using System.Collections.Generic;

public class FixedSizeQueue<T>
{
    private readonly int _maxSize;
    private readonly Queue<T> _queue;

    public FixedSizeQueue(int maxSize)
    {
        _maxSize = maxSize;
        _queue = new Queue<T>(maxSize);
    }

    public void Enqueue(T item)
    {
        if (_queue.Count >= _maxSize)
        {
            _queue.Dequeue();
        }

        _queue.Enqueue(item);   
    }

    public T Dequeue()
    {
        T item = _queue.Dequeue();
        return item;
    }
}
