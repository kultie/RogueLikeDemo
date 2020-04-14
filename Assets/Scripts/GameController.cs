using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    List<Entity> entities = new List<Entity>();
    private void Awake()
    {
        Instance = this;
        CharacterController a = new GameObject("Character").AddComponent<CharacterController>();
        a.Setup(5, 5, 0.8f);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        var arr = entities.ToArray();
        for (int i = arr.Length - 1; i >= 0; --i) {
            arr[i].ManualUpdate(dt);
        }
    }

    private void FixedUpdate()
    {
        float dt = Time.deltaTime;
        var arr = entities.ToArray();
        for (int i = arr.Length - 1; i >= 0; --i)
        {
            arr[i].ManualFixedUpdate(dt);
        }
    }

    public void AddEntity(Entity e) {
        entities.Add(e);
    }

    public void RemoveEntity(Entity e) {
        entities.Remove(e);
    }
}
