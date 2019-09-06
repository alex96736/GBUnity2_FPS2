using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : BaseWeapon
{
    // дестанция стрельбы
    private float _shootDistance = 1000f;

    // Базовый урон
    private int _damageBase = 20;
    // урон
    private int _damage = 20;
    // штраф урона за стены
    private int _damageWall = 1;

    private float _destructTime = 10;

    // кол-во патронов, которое помещяется в магазин
    [SerializeField] private int _magazine = 30;
    // текущее количество патронов в магазине
    [SerializeField] private int _bulletsInMagazine = 30;
    // количество патронов вне магазина
    [SerializeField] private int _bulletsOutMagazine = 30;

    //public Bullet _bullet;





    protected override void Update()
    {
        UIText_Bullets.text = $"{_bulletsInMagazine} / {_bulletsOutMagazine}";

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    /// <summary>
    /// Стрельба
    /// </summary>
    public override void Fire()
    {
        if (_bulletsInMagazine > 0 && _fire)
        {
            _muzzleFlash.Play();

            _bulletsInMagazine--;

            #region FirePrefab
            //Bullet tempBullet = Instantiate(_bullet, _SpawnBullet.position, _SpawnBullet.rotation) as Bullet;
            //if (tempBullet)
            //{
            //    RaycastHit hit2;
            //    Ray ray2 = _mainCamera.ScreenPointToRay(_crossHair.transform.position);
            //    Rigidbody _bulletRigidbody = tempBullet.GetComponent<Rigidbody>();
            //    if (Physics.Raycast(ray2, out hit2, _shootDistance))
            //    {
            //        _bulletRigidbody.AddForce(GetDirection(hit2.point, tempBullet.transform.position) * 1000, ForceMode.Impulse);
            //    }
            //    else
            //    {
            //        _bulletRigidbody.AddForce(GetDirection(ray2.GetPoint(10000f), tempBullet.transform.position) * 1000, ForceMode.Impulse);
            //    }
            //}
            #endregion

            RaycastHit hit;
            Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
            if (Physics.Raycast(ray, out hit, _shootDistance))
            {
                if (hit.collider.tag == "Player")
                {
                    return;
                }
                else
                {
                    LogMassivRayCast(ray);
                    //Debug.Log(Physics.RaycastAll(ray, _shootDistance).Length);
                    foreach (RaycastHit temp in Physics.RaycastAll(ray, _shootDistance))
                    {
                        //Debug.Log(temp.collider.tag);
                        if (temp.collider.tag == "Enemy" && _damage > 0)
                        {
                            Debug.Log($"Set Damage: {_damage}");
                            SetDamage(temp.collider.GetComponent<ISetDamage>());
                            _damage = _damageBase;
                            return;
                        }
                        else
                        {
                            //Debug.Log($"Name: {temp.collider.name}");
                            _damage -= _damageWall;
                        }
                    }
                }
                GameObject TempHit = Instantiate(_hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                TempHit.transform.parent = hit.transform;
                Destroy(TempHit, 0.5f);
            }

            Debug.Log(Physics.RaycastAll(ray, _shootDistance).Length);

            if (_bulletsInMagazine == 0 && _bulletsOutMagazine > 0)
            {
                Reload();
            }
        }
    }

    public void LogMassivRayCast(Ray ray)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, _shootDistance);
        int i = 1;
        foreach (RaycastHit temp in hits)
        {

            Debug.Log($"{i}: {temp.collider.name}");
            i++;
        }
    }

    // направление, в котором летит пуля
    //Vector3 GetDirection(Vector3 HitPoint, Vector3 BulletPos)
    //{
    //    Vector3 decr = HitPoint - BulletPos;
    //    float dist = decr.magnitude;
    //    return decr / dist;
    //}

    /// <summary>
    /// Перезарядка
    /// </summary>
    public void Reload()
    {
        if (_bulletsOutMagazine > 0)
        {
            // разница между патронами в магазине и максимальной вместительностью магазина
            int bulletsDifference = _magazine - _bulletsInMagazine;

            if (bulletsDifference > _bulletsOutMagazine)
            {
                bulletsDifference = _bulletsOutMagazine;
            }

            _bulletsInMagazine = _bulletsInMagazine + bulletsDifference;
            _bulletsOutMagazine = _bulletsOutMagazine - bulletsDifference;
        }
    }

    private void SetDamage(ISetDamage obj)
    {
        if (obj != null)
        {
            obj.SetDamage(_damage);
        }
    }
}
