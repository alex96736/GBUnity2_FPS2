using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private Transform _playerPos;

    void Start()
    {
        _playerPos = GameObject.FindObjectOfType<Player>().transform;

    }

    void LateUpdate()
    {
        Vector3 newPosition = _playerPos.position;
        newPosition.y = transform.position.y;

        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, _playerPos.eulerAngles.y, 0f);
    }
}
