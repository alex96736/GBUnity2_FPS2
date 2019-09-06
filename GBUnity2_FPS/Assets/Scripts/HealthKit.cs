using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : BaseObject
{
    private int _healthPoints;

    protected override void Awake()
    {
        base.Awake();
        _healthPoints = 25;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Player")
        {
            GetHealth(other.GetComponent<IGetHealth>());
            Debug.Log($"{other.tag} get health: {_healthPoints}");
            Destroy(gameObject);
        }
    }

    private void GetHealth(IGetHealth obj)
    {
        if (obj != null)
        {
            obj.GetHealth(_healthPoints);
        }
    }
}
