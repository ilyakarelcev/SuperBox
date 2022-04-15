using UnityEngine;

[System.Serializable]
public class PlayerStaticInfo : ISengleTone
{
    [SerializeField] private Player _player;

    public static Player Player { get; private set; }

    public void Init()
    {
        Player = _player;
    }
}