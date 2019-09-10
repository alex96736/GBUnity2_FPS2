using UnityEditor;
using UnityEngine;

public class Window : EditorWindow
{

    public GameObject botPrefab;
    string _name = "Bot";
    public int objectCounter;
    public float radius = 20;

    [MenuItem("Создание префабов/ Окно генератора ботов")]
    public static void ShowWindow()
    {
        GetWindow(typeof(Window));
    }

    private void OnGUI()
    {
        GUILayout.Label("Настройки", EditorStyles.boldLabel);

        botPrefab = EditorGUILayout.ObjectField("Префаб бота", botPrefab, typeof(GameObject), true) as GameObject;

        objectCounter = EditorGUILayout.IntSlider("Количество объектов", objectCounter, 1, 200);
        radius = EditorGUILayout.Slider("Радиус", radius, 10, 100);

        if (GUILayout.Button("Сгенерировать ботов"))
        {
            if (botPrefab)
            {
                GameObject Main = new GameObject("MainBot");
                for (int i = 0; i < objectCounter; i++)
                {
                    float angle = i * Mathf.PI * 2 / objectCounter;

                    Vector3 _position = (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius);
                    GameObject temp = Instantiate(botPrefab, _position, Quaternion.identity);
                    temp.transform.parent = Main.transform;
                    temp.name += "("+i+")";
                }
            }
        }
    }
}
