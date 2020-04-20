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
        if (controller != null)
        {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (controller != null)
        {
            ICollision controllerInteraction = controller as ICollision;
            if (controllerInteraction != null)
            {
                controllerInteraction.EnterCollision(collision.gameObject.GetComponent<Entity>());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (controller != null)
        {
            ICollision controllerInteraction = controller as ICollision;
            if (controllerInteraction != null)
            {
                controllerInteraction.ExitCollision(collision.gameObject.GetComponent<Entity>());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (controller != null)
        {
            ITriggerCollision controllerInteraction = controller as ITriggerCollision;
            if (controllerInteraction != null)
            {
                controllerInteraction.EnterTriggerCollision(collision.gameObject.GetComponent<Entity>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (controller != null)
        {
            ITriggerCollision controllerInteraction = controller as ITriggerCollision;
            if (controllerInteraction != null)
            {
                controllerInteraction.ExitTriggerCollision(collision.gameObject.GetComponent<Entity>());
            }
        }
    }
}
