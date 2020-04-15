using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kultie.DungeonSystem;
using Com.LuisPedroFonseca.ProCamera2D;
public class GameController : MonoBehaviour
{
    public Transform gridContainer;

    public static GameController Instance;
    List<Entity> entities = new List<Entity>();
    public ProCamera2D cam { private set; get; }
    DungeonGeneration d;
    private void Awake()
    {
        cam = Camera.main.GetComponent<ProCamera2D>();
        Instance = this;
        CharacterController a = new GameObject("Character").AddComponent<CharacterController>();
        cam.AddCameraTarget(a.transform);
        a.Setup(5, 5, 0.8f);
        d = new DungeonGeneration(50, 50);
        CreateMap();
    }

    void CreateMap()
    {
        d.CreateMap();
        d.IterateMap(SpawnTile);
    }

    void SpawnTile(DungeonTile tile)
    {
        Entity a = new GameObject("Tile(" + tile.x + ":" + tile.y).AddComponent<Entity>();
        a.transform.SetParent(gridContainer);
        a.transform.position = new Vector2(tile.x - 25, tile.y - 25);
        if (tile.type == TileType.WALL)
        {
            a.SetColor(Color.black);
        }
    }

    void Update()
    {
        InputHandleUtilities.UpdateInput();
        float dt = Time.deltaTime;
        var arr = entities.ToArray();
        for (int i = arr.Length - 1; i >= 0; --i)
        {
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

    public void AddEntity(Entity e)
    {
        entities.Add(e);
    }

    public void RemoveEntity(Entity e)
    {
        entities.Remove(e);
    }
}
