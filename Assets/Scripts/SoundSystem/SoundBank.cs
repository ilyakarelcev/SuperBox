using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Bank", menuName = "Sound Bank", order = 51)]
public class SoundBank : ScriptableObject
{
    [Header("Box")]
    public SoundSetup Jump;
    public SoundSetup BoxAttack;
    public SoundSetup PhysicsContact;
    public SoundSetup SlowMotion;
    [Space]
    public SoundSetup FireAbility;
    public SoundSetup WindAbility;

    [Header("General")]
    public SoundSetup Hit;
    public SoundSetup DieSound;

    [Header("Enemy")]
    public SoundSetup Awake;

    [Header("Sworder")]
    public SoundSetup SwordAttack;
    public SoundSetup HitOnDontBreakState;

    [Header("Shotter")]
    public SoundSetup Shoot;
    public SoundSetup BulletDestroy;

    [Header("Magic")]
    public SoundSetup FireCast;

    [Header("Triceratops")]
    public SoundSetup JerkBegin;
    public SoundSetup TriceratopsAttack;
}
