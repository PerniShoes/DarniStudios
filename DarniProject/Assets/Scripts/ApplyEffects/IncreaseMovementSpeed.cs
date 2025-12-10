using UnityEngine;
public class IncreaseMovementSpeed : ApplyEffect
{
    public override void Apply(PlayerXP player, float value)
    {
        player.movementStats.topSpeed += value;
    }
}

