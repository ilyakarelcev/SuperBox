using UnityEngine;

[System.Serializable]
public class PlayerStaticInfo : ISengleTone
{
    [SerializeField] private Player _player;

    public static Player Player { get; private set; }

    public void Init()
    {
        if (Player)
            Debug.LogError("More one Instance of Singletone");

        Player = _player;
    }
}