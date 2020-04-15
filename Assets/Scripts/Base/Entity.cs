﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    SpriteRenderer render;

    protected virtual void Awake()
    {
        render = gameObject.AddComponent<SpriteRenderer>();
        GenerateTemplate();
        GameController.Instance.AddEntity(this);
    }

    public void SetSprite(Sprite s)
    {
        render.sprite = s;
    }

    private void GenerateTemplate()
    {
        SetSprite(ResourceManager.GetSprite("New Piskel",0,"Texture"));
    }

    public virtual void ManualUpdate(float dt)
    {

    }

    public virtual void ManualFixedUpdate(float dt)
    {

    }
}
