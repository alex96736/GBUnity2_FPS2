using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseObject
{
    private int _damage = 20;
    private float _destructTime = 10;

    protected override void Awake()
    {
        base.Awake();
        Destroy(_GameObject, _destructTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            return;
        }
        SetDamage(collision.gameObject.GetComponent<ISetDamage>());
        Destroy(_GameObject);
    }

    private void SetDamage(ISetDamage obj)
    {
        if (obj != null)
        {
            obj.SetDamage(_damage);
        }
    }

    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
