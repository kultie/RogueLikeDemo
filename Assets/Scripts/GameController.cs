using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kultie.DungeonSystem;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    [SerializeField]
    int mapWidth;
    [SerializeField]
    int mapHeight;

    public Tile groundTile;
    public Tilemap groundLayer;

    public RuleTile wallTile;
    public Tilemap wallLayer;

    Vector3Int mapCenterOffSet;

    DungeonGeneration d;
    public static GameController Instance;
    CharacterController characterController;
    public ProCamera2D cam { private set; get; }

    private void Awake()
    {
        Instance = this;

        cam = Camera.main.GetComponent<ProCamera2D>();
        CreateMap();
        CreateTemplateCharacter();
    }

    void CreateTemplateCharacter()
    {
        RigidEntity a = new GameObject("Character").AddComponent<RigidEntity>();
        characterController = new CharacterController(a, "template");
        a.transform.position = d.mainCave.GetRandomTile().GetPosition3() - mapCenterOffSet;
        cam.AddCameraTarget(a.transform);
        //RigidEntity b = new GameObject("Character").AddComponent<RigidEntity>();
        //b.SetController(new InverseCharacterController(b, "template_2"));
        //b.transform.position = Vector2.one;
        //cam.AddCameraTarget(b.transform);
    }

    void CreateMap()
    {
        d = new DungeonGeneration(mapWidth, mapHeight);
        mapCenterOffSet = new Vector3Int(mapWidth / 2, mapHeight / 2, 0);
        d.CreateMap();
        d.IterateMap(SpawnTile);
        var boundary = cam.GetComponent<ProCamera2DNumericBoundaries>();
        boundary.TopBoundary = mapHeight / 2;
        boundary.BottomBoundary = -mapHeight / 2;
        boundary.RightBoundary = mapWidth / 2;
        boundary.LeftBoundary = -mapWidth / 2;
    }

    /// <summary>
    /// Spawn Tile for dungeon after generated
    /// </summary>
    /// <param name="tile"></param>
    void SpawnTile(DungeonTile tile)
    {
        if (tile.type == TileType.WALL)
        {
            wallLayer.SetTile(tile.GetPosition3() - mapCenterOffSet, wallTile);
        }
        else
        {
            groundLayer.SetTile(tile.GetPosition3() - mapCenterOffSet, groundTile);
        }
    }

    void Update()
    {
        InputHandleUtilities.UpdateInput();
        float dt = Time.deltaTime;
        characterController.Update(dt);
    }
}
