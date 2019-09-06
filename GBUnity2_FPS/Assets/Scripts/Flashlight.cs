using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, описывающий поведение фонаря
/// </summary>
public class Flashlight : BaseObject
{
    // клавиша активациии фонаря
    private KeyCode control = KeyCode.F;

    // текущий заряд батареи
    [SerializeField] private float battery;
    // максимальный заряд батареи
    [SerializeField] private float batteryMax = 100;

    [SerializeField] private Text UI_BatteryValue;

    private Light _light;
    private Material material;
    private Material _lightMat;

    // Уровни батареи для изменения силы света фонаря
    private float[] BatteryLightLevels = { 20, 50, 80};

    // Уровни баатреи для изменения силы света фонаря
    private float[] IntensityLightLevels = { 1.5f, 3, 4, 5 };





    void Start()
    {
        _light = GetComponentInChildren<Light>();
        _lightMat = GetMaterial;
    }

    void Update()
    {
        // вкл/выкл
        if (Input.GetKeyDown(control) && !_light.enabled)
        {
            ActiveFlashlight(true);
        }
        else if (Input.GetKeyDown(control) && _light.enabled)
        {
            ActiveFlashlight(false);
        }

        // поведение заряда батареи
        if (_light.enabled)
        {
            battery = battery - Time.deltaTime*10;
            IntensityLight();
            if (battery <= 0)
            {
                ActiveFlashlight(false);
            }
        }
        else
        {
            if (battery < batteryMax)
            {
                battery += Time.deltaTime*10;
            }
        }

        // обновление значений в UI
        UI_BatteryValue.text = Mathf.Round(battery).ToString();
    }

    /// <summary>
    /// Функция вкл/выкл фонаря
    /// </summary>
    private void ActiveFlashlight(bool value)
    {
        _light.enabled = value;
    }

    /// <summary>
    /// Смена силы света фоноря, в зависимости от заряда батареи
    /// </summary>
    private void IntensityLight()
    {
        if (battery >= BatteryLightLevels[2])
        {
            _light.intensity = IntensityLightLevels[3];
        }
        else if (battery < BatteryLightLevels[2] && battery >= BatteryLightLevels[1])
        {
            _light.intensity = IntensityLightLevels[2];
        }
        else if (battery < BatteryLightLevels[1] && battery >= BatteryLightLevels[0])
        {
            _light.intensity = IntensityLightLevels[1];
        }
        else 
        {
            _light.intensity = IntensityLightLevels[0];
        }
    }
}
