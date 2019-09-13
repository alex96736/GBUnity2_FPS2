using System;
using System.IO;
using UnityEngine;

public class JSONData : ISaveData
{
    string _path = Path.Combine(Application.dataPath, "JSONData.xml");

    public void Save(PlayerStruct _player)
    {
        string FileJson = JsonUtility.ToJson(_player);
        File.WriteAllText(_path, FileJson);

    }

    public PlayerStruct Load()
    {
        if (File.Exists(_path))
        {
            string temp = File.ReadAllText(_path);
            return JsonUtility.FromJson<PlayerStruct>(temp);
        }
        else
        {
            PlayerStruct result = new PlayerStruct();
            Debug.Log("Не задан путь");
            return result;
        }
    }
}
