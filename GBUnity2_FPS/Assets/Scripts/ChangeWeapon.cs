using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Класс, описывающий смену оружия
/// </summary>
public class ChangeWeapon : BaseObject
{
    // id выбранного оружия
    private int weaponID = 0;

    void Start()
    {
        SelectWeapon();
    }

    /// <summary>
    /// переключение оружия
    /// </summary>
    private void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in _GameObjectTransform)
        {
            if (i == weaponID)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    void Update()
    {
        // id предыдущего оружия
        int previousSelectWeapon = weaponID;

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (weaponID <= 0)
            {
                weaponID = ChildCount - 1;
            }
            else
            {
                weaponID--;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (weaponID >= ChildCount - 1)
            {
                weaponID = 0;
            }
            else
            {
                weaponID++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponID = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && ChildCount > 2)
        {
            weaponID = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && ChildCount > 3)
        {
            weaponID = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && ChildCount > 4)
        {
            weaponID = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && ChildCount > 5)
        {
            weaponID = 4;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) && ChildCount > 6)
        {
            weaponID = 5;
        }

        if (previousSelectWeapon != weaponID)
        {
            SelectWeapon();
        }
    }
}
