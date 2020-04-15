using SimpleJSON;
public class EntityInfo
{
    public string texture { private set; get; }
    public int startFrame { private set; get; }

    public EntityInfo(string entityID)
    {
        JSONNode data = ResourceManager.GetEntitesData("Entities", entityID, "JsonData");
        texture = data["texture"];
        startFrame = data["start_frame"].AsInt;
    }
}