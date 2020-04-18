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

    private void Awake()
    {
        cam = Camera.main.GetComponent<ProCamera2D>();
        Instance = this;
        CreateMap();
        CreateTemplateCharacter();
    }

    void CreateTemplateCharacter()
    {
        RigidEntity a = new GameObject("Character").AddComponent<RigidEntity>();
        a.SetController(new CharacterControllerBase(a, "template"));
        cam.AddCameraTarget(a.transform);
        //RigidEntity b = new GameObject("Character").AddComponent<RigidEntity>();
        //b.SetController(new InverseCharacterController(b, "template_2"));
        //b.transform.position = Vector2.one;
        //cam.AddCameraTarget(b.transform);
    }

    void CreateMap()
    {
        DungeonGeneration d = new DungeonGeneration(100, 100);
        d.CreateMap();
        d.IterateMap(SpawnTile);
    }

    /// <summary>
    /// Spawn Tile for dungeon after generated
    /// </summary>
    /// <param name="tile"></param>
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
