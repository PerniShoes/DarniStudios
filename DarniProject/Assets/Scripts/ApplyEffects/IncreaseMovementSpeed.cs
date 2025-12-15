using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseMovementSpeed", menuName = "LevelUp/Effects/IncreaseMovementSpeed")]
public class IncreaseMovementSpeed : ApplyEffect
{
    public override void Apply(PlayerXP player, float value)
    {
        player.movementStats.topSpeed += value;
    }
}

