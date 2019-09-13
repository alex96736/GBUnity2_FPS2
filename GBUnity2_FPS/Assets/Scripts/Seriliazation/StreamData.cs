using System;
using UnityEngine;
using System.IO;

public class StreamData : ISaveData
{
    private string _path = Path.Combine(Application.dataPath, "Stream.xyz");

    public void Save(PlayerStruct _player)
    {
        using (StreamWriter TempSteamWriter = new StreamWriter(_path))
        {
            TempSteamWriter.WriteLine(_player.Name);
            TempSteamWriter.WriteLine(_player.Health);
            TempSteamWriter.WriteLine(_player.Visible);
        }
    }

    public PlayerStruct Load()
    {
        var result = new PlayerStruct();
        if (!File.Exists(_path))
        {
            Debug.Log("Не задан путь");
            return result;
        }
        using (StreamReader TempStreamReader = new StreamReader(_path))
        {
            while (!TempStreamReader.EndOfStream)
            {
                result.Name = TempStreamReader.ReadLine();
                Int32.TryParse(TempStreamReader.ReadLine(), out result.Health);
                Boolean.TryParse(TempStreamReader.ReadLine(), out result.Visible);
            }
        }
        return result;
    }
}
