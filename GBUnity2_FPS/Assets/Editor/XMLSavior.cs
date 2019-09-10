using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.SceneManagement;

[Serializable]
public struct SVector3
{
    public float X;
    public float Y;
    public float Z;

    public SVector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static implicit operator SVector3(Vector3 value)
    {
        return new SVector3(value.x, value.y, value.z);
    }

    public static implicit operator Vector3(SVector3 value)
    {
        return new Vector3(value.X, value.Y, value.Z);
    }
}

public struct SQuaternion
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    public SQuaternion(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static implicit operator SQuaternion(Quaternion value)
    {
        return new SQuaternion(value.x, value.y, value.z, value.w);
    }

    public static implicit operator Quaternion(SQuaternion value)
    {
        return new Quaternion(value.X, value.Y, value.Z, value.W);
    }
}

public struct SGameObject
{
    public string Name;
    public SVector3 Position;
    public SVector3 Scale;
    public SQuaternion Rotation;
}

public class SaveLavel
{
    [MenuItem("Сохранение шаблона/ сохранить уровень", false, 1)]
    private static void SaveScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        List<GameObject> rootObject = new List<GameObject>();
        scene.GetRootGameObjects(rootObject);
        List<SGameObject> levelObjectList = new List<SGameObject>();
        string savePath = Path.Combine(Application.dataPath, "EditorData.xml");
        foreach (var obj in rootObject)
        {
            var temp = obj.transform;
            levelObjectList.Add(new SGameObject
            {
                Name = obj.name,
                Position = temp.position,
                Rotation = temp.rotation,
                Scale = temp.localScale
            });
        }
        XMLSavior.Save(levelObjectList.ToArray(),savePath);
    }
}

public class XMLSavior : MonoBehaviour
{
    private static XmlSerializer _formator;

    static XMLSavior()
    {
        _formator = new XmlSerializer(typeof(SGameObject[]));
    }

    public static void Save(SGameObject[] levelObjects, string path)
    {
        if(levelObjects==null && !String.IsNullOrEmpty(path))
        {
            Debug.Log("Не задан путь или массив пуст");
            return;
        }
        if (levelObjects.Length<=0)
        { return; }

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            _formator.Serialize(fs, levelObjects);
        }
    }

    [MenuItem("Сохранение шаблона/ загрузить уровень", false, 1)]
    private static void LoadTempScene()
    {
        SGameObject[] result;
        using (FileStream fs = new FileStream(Path.Combine(Application.dataPath, "EditorData.xml"), FileMode.Open))
        {
            result = (SGameObject[])_formator.Deserialize(fs);
        }

        foreach (var obj in result)
        {
            var _prefab = Resources.Load<GameObject>("Prefabs/LoadedObject" + obj.Name);
            if (_prefab != null)
            {
                GameObject temp = Instantiate(_prefab, obj.Position, obj.Rotation);
                temp.name = obj.Name;
            }
        }
    }
}
