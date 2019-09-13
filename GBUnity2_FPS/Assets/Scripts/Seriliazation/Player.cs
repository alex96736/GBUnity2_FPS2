using UnityEngine;

public struct PlayerStruct
{
    public string Name;
    public int Health;
    public bool Visible;

    public override string ToString() => $"Name{Name} Health{Health} Visible{Visible}";
}

public class Player : BaseObject
{
    private int _health;
    private ISaveData _data;

    void Start()
    {
        _health = 100;
        _data = new XMLData();
        PlayerStruct player = new PlayerStruct
        {
            Name = name,
            Health = _health,
            Visible = _isVisible
        };
        _data.Save(player);
        PlayerStruct newPlayer = _data.Load();
        Debug.Log(newPlayer);
    }

}
