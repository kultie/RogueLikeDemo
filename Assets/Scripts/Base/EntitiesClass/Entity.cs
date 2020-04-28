using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    SpriteRenderer render;

    protected virtual void Awake()
    {
        render = gameObject.AddComponent<SpriteRenderer>();
        GenerateTemplate();
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
}
