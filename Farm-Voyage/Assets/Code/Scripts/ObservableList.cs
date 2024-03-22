using System;
using System.Collections.Generic;

public class ObservableList<T>
{
    public event Action<T> OnItemAdded;
    public event Action<T> OnItemRemoved;
    
    private readonly List<T> _list = new();

    public void Add(T item)
    {
        _list.Add(item);
        OnItemAdded?.Invoke(item);
    }

    public void Remove(T item)
    {
        if (_list.Remove(item))
        {
            OnItemRemoved?.Invoke(item);
        }
    }

    public T this[int i]
    {
        get => _list[i];
        set => _list[i] = value;
    }

    public int Count => _list.Count;

    public bool Contains(T item) => _list.Contains(item);
}
