using System.Linq;
using UnityEngine;

public class SengletoneManager : MonoBehaviour
{
    [SerializeField] private PlayerStaticInfo _playerInfo;
    [SerializeField] private Sound _sound;
    [Space]
    [SerializeField] private Component[] _singleTonesGO;

    private ISengleTone[] _singleTones;

    private void Awake()
    {
        _singleTones = _singleTonesGO.Where(x => x is ISengleTone).Select(x => (ISengleTone)x).ToArray();
        foreach (var singleTone in _singleTones)
        {
            singleTone.Init();
        }

        _playerInfo.Init();
        _sound.Init();
    }
}