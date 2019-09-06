using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, описыаающйи поведение мины
/// </summary>
public class Mine : BaseObject
{
    // радиус взрыва мины
    [SerializeField] private float _radius;
    // сила отброса от взрыва
    private int power = 500;
    // Базовый урон
    private int _damageBase = 30;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    protected override void Awake()
    {
        base.Awake();
        _radius = 1.5f;
    }

    private void Update()
    {
        foreach (Collider collision in Physics.OverlapSphere(_GameObjectTransform.position, _radius))
        {
            if (collision.tag == "Player" || collision.tag == "Enemy")
            {
                Explosion();
                Debug.Log("Mine explosion");
            }
        }
    }

    /// <summary>
    /// Врзыв мины
    /// </summary>
    public void Explosion()
    {
        foreach (Collider col in Physics.OverlapSphere(_GameObjectTransform.position, _radius))
        {
            SetDamage(col.gameObject.GetComponent<ISetDamage>());
            //if (col.gameObject.GetComponent<Rigidbody>())
            //{
            //    col.gameObject.GetComponent<Rigidbody>().AddForce((col.gameObject.transform.position - gameObject.transform.position) * power, ForceMode.Impulse);
            //    SetDamage(col.gameObject.GetComponent<ISetDamage>());
            //}
        }
        Destroy(gameObject);
    }

    private void SetDamage(ISetDamage obj)
    {
        if (obj != null)
        {
            obj.SetDamage(_damageBase);
        }
    }
}
