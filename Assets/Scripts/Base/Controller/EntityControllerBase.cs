using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityControllerBase
{
    public Entity entity { private set; get; }
    public EntityControllerBase(Entity e)
    {
        entity = e;
        Initialize();
    }

    public abstract void Update(float dt);

    protected abstract void Initialize();
}
