using System;
using UnityEngine;
using System.IO;
using System.Xml;

public class XMLData : ISaveData
{
    private string _path = Path.Combine(Application.dataPath, "XMLData.xml");

    public void Save(PlayerStruct _player)
    {
        XmlDocument xmlDoc = new XmlDocument();

        XmlNode rootNode = xmlDoc.CreateElement("Player");
        xmlDoc.AppendChild(rootNode);

        XmlElement element = xmlDoc.CreateElement("Name");
        element.SetAttribute("value", _player.Name);
        rootNode.AppendChild(element);

        element = xmlDoc.CreateElement("Health");
        element.SetAttribute("value", _player.Health.ToString());
        rootNode.AppendChild(element);

        element = xmlDoc.CreateElement("Visible");
        element.SetAttribute("value", _player.Visible.ToString());
        rootNode.AppendChild(element);

        xmlDoc.Save(_path);
    }

    public PlayerStruct Load()
    {
        var result = new PlayerStruct();

        if (!File.Exists(_path))
        {
            Debug.Log("Не задан путь");
            return result;
        }
        using (XmlTextReader reader = new XmlTextReader(_path))
        {
            string key = "Name";
            while (reader.Read())
            {
                if (reader.IsStartElement(key))
                {
                    result.Name = reader.GetAttribute("value");
                }
                key = "Health";
                if (reader.IsStartElement(key))
                {
                    Int32.TryParse(reader.GetAttribute("value"), out result.Health);
                }
                key = "Visible";
                if (reader.IsStartElement(key))
                {
                    Boolean.TryParse(reader.GetAttribute("value"), out result.Visible);
                }
            }
        }
        return result;
    }
}
