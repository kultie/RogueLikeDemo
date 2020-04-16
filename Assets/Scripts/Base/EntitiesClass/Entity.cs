using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    SpriteRenderer render;
    protected EntityControllerBase controller;

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

    public void SetColor(Color col)
    {
        render.color = col;
    }

    public void SetOrderInLayer(int value)
    {
        render.sortingOrder = value;
    }

    private void GenerateTemplate()
    {
        SetSprite(ResourceManager.GetSprite("New Piskel", 0, "Texture"));
    }

    public virtual void ManualUpdate(float dt)
    {
        if (controller != null) {
            controller.Update(dt);
        }
    }

    public virtual void ManualFixedUpdate(float dt)
    {

    }

    public void SetController(EntityControllerBase c)
    {
        controller = c;
    }

    private void OnDisable()
    {
        GameController.Instance.RemoveEntity(this);
    }
}
