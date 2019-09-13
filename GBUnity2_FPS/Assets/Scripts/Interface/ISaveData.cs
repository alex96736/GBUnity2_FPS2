using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveData
{
    void Save(PlayerStruct _player);

    PlayerStruct Load();

}
