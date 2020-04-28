using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityControllerBase<T> where T : Entity
{
    public T entity { private set; get; }
    public EntityControllerBase(T e, string resourceName)
    {
        entity = e;
        Initialize(resourceName);
    }

    public abstract void Update(float dt);

    protected abstract void Initialize(string resourceName);
}
