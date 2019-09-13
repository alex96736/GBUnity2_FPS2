using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerPlus : Unit
{
    [Header("Информация о здоровье игрока")]
    // Вывод информации о здоровье
    [SerializeField] protected Text UIText_Health;

    protected override void Awake()
    {
        base.Awake();
        BaseHealth = 100;
        Health = BaseHealth;
    }

    private void Update()
    {
        UIText_Health.text = Health.ToString();
    }
}
