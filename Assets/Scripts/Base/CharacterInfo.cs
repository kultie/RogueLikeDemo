using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo
{
    public EntityInfo entity { private set; get; }
    public Dictionary<string, Sprite[]> animationData { private set; get; }
    public Dictionary<Facing, Sprite> facingSprite { private set; get; }
    public Facing startFacing { private set; get; }
    public float accelerationRate { private set; get; }
    public float friction { private set; get; }
    public float maxSpeed { private set; get; }
    public CharacterInfo(string characterID)
    {
        JSONNode data = ResourceManager.GetCharacterData("Characters", characterID, "JsonData");
        entity = new EntityInfo(data["entity"]);
        InitializeAnimationData(data["anims"].AsObject);
        InitializeFacingSprites(data["facing"].AsObject);
        startFacing = GetStartFacing(data["start_facing"]);
        accelerationRate = data["acceleration_rate"].AsFloat;
        friction = data["friction"].AsFloat;
        maxSpeed = data["max_speed"].AsFloat;

    }

    void InitializeAnimationData(JSONClass data)
    {
        animationData = new Dictionary<string, Sprite[]>();
        foreach (KeyValuePair<string, JSONNode> tmp in data)
        {
            JSONArray tmp_data = tmp.Value.AsArray;
            Sprite[] animArray = new Sprite[tmp_data.Count];
            for (int i = 0; i < animArray.Length; ++i)
            {
                animArray[i] = GetSprite(tmp_data[i].AsInt);
            }
            animationData[tmp.Key] = animArray;
        }
    }

    void InitializeFacingSprites(JSONClass data)
    {
        facingSprite = new Dictionary<Facing, Sprite>();
        foreach (KeyValuePair<string, JSONNode> tmp in data)
        {
            switch (tmp.Key)
            {
                case "up":
                    facingSprite[Facing.UP] = GetSprite(tmp.Value.AsInt);
                    break;
                case "down":
                    facingSprite[Facing.DOWN] = GetSprite(tmp.Value.AsInt);
                    break;
                case "left":
                    facingSprite[Facing.LEFT] = GetSprite(tmp.Value.AsInt);
                    break;
                case "right":
                    facingSprite[Facing.RIGHT] = GetSprite(tmp.Value.AsInt);
                    break;
            }
        }
    }

    Facing GetStartFacing(JSONNode data)
    {
        switch (data["start_facing"])
        {
            case "down":
                return Facing.DOWN;
            case "left":
                return Facing.LEFT;
            case "up":
                return Facing.UP;
            case "right":
                return Facing.RIGHT;
            default:
                return Facing.DOWN;
        }
    }

    Sprite GetSprite(int index)
    {
        return ResourceManager.GetSprite(entity.texture, index, "Texture");
    }
}