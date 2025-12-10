using UnityEngine;

public abstract class ApplyEffect : ScriptableObject
{
    public abstract void Apply(PlayerXP playerLevelManager, int value);
}
