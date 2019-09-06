using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : BaseObject, ISetDamage, IGetHealth
{
    // текущее здоровье персонажа
    [SerializeField] private int _health;
    // базовое здоровье персонажа
    private int _baseHealth;


    public int Health { get => _health; set => _health = value; }
    public int BaseHealth { get => _baseHealth; set => _baseHealth = value; }


    public void SetDamage(int damage)
    {
        if (_health > damage)
        {
            _health -= damage;
            Debug.Log($"SetDamage: {damage}");
        }
        else
        {
            _health = 0;
            Destroy(_GameObject);
        }
        Debug.Log($"Health: {_health}");
    }

    public void GetHealth(int health)
    {
        _health += health;
        if (_health > _baseHealth)
        {
            _health = _baseHealth;
        }
    }
}
