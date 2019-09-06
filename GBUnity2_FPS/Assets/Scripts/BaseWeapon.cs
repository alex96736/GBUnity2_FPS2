using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Базовый класс для оружия
/// </summary>
public abstract class BaseWeapon : BaseObject
{
    // точка, из которой производится выстрел
    protected Transform _SpawnBullet;
    // сила выстрела
    protected float _force;
    // прицел
    protected GameObject _crossHair;

    // Время задержки между выстрелами           
    protected Timer _rechargeTime = new Timer();
    // Флаг, для разрешения стрельбы
    protected bool _fire = true;

    protected ParticleSystem _muzzleFlash;
    //protected Light _mazzleLight;
    [SerializeField]protected GameObject _hitParticle;

    // Вывод информации о боезапасе
    [SerializeField] protected Text UIText_Bullets;

    public abstract void Fire();

    protected override void Awake()
    {
        base.Awake();
        _SpawnBullet = _GameObjectTransform.GetChild(2);
        _crossHair = GameObject.FindGameObjectWithTag("CrossHair");
        _muzzleFlash = GetComponentInChildren<ParticleSystem>();
    }

    protected virtual void Update()
    {
        _rechargeTime.Update();

        if (_rechargeTime.IsEvent())
        {
            _fire = true;
        }
    }
}
