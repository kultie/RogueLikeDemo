using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public static class ResourceManager
{
    private static Dictionary<string, Sprite[]> spriteCache = new Dictionary<string, Sprite[]>();
    private static Dictionary<string, TextAsset> textCache = new Dictionary<string, TextAsset>();
    private static Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();
    private static Dictionary<string, JSONNode> characterCache = new Dictionary<string, JSONNode>();
    private static Dictionary<string, JSONNode> entityCache = new Dictionary<string, JSONNode>();

    public static Sprite[] GetSprite(string resourceName, string folderName = null)
    {
        string key = GetKey(folderName, resourceName);
        if (!spriteCache.TryGetValue(key, out Sprite[] result))
        {
            Sprite[] tmp = Resources.LoadAll<Sprite>(key);
            spriteCache[key] = tmp;
            result = tmp;
        }
        return result;
    }

    public static Sprite GetSprite(string resourceName, int index, string folderName = null)
    {
        Sprite[] result = GetSprite(resourceName, folderName);
        return result[index];
    }

    public static TextAsset GetTextAsset(string resourceName, string folderName = null)
    {
        string key = GetKey(folderName, resourceName);

        if (!textCache.TryGetValue(key, out TextAsset result))
        {
            TextAsset tmp = Resources.Load<TextAsset>(key);
            textCache[key] = tmp;
            result = tmp;
        }
        return result;
    }

    public static AudioClip GetAudioClip(string resourceName, string folderName = null)
    {
        string key = GetKey(folderName, resourceName);

        if (!audioCache.TryGetValue(key, out AudioClip result))
        {
            AudioClip tmp = Resources.Load<AudioClip>(key);
            audioCache[key] = tmp;
            result = tmp;
        }
        return result;
    }

    public static JSONNode GetCharacterData(string resourceName, string characterID, string folderName = null)
    {
        string key = GetKey(folderName, resourceName);

        if (!characterCache.TryGetValue(key, out JSONNode result))
        {
            TextAsset data = Resources.Load<TextAsset>(key);
            JSONNode tmp = JSON.Parse(data.text);
            characterCache[key] = tmp;
            result = tmp;
        }
        return result[characterID];
    }

    public static JSONNode GetEntitesData(string resourceName, string entityID, string folderName = null)
    {
        string key = GetKey(folderName, resourceName);

        if (!entityCache.TryGetValue(key, out JSONNode result))
        {
            TextAsset data = Resources.Load<TextAsset>(key);
            JSONNode tmp = JSON.Parse(data.text);
            entityCache[key] = tmp;
            result = tmp;
        }
        return result[entityID];
    }

    private static string GetKey(string folderName, string resourceName)
    {
        string key = resourceName;
        if (folderName != null)
        {
            key = folderName + "/" + resourceName;
        }
        return key;
    }
}
