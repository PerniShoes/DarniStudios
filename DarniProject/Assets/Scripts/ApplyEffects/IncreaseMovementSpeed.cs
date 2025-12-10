using UnityEngine;
public class IncreaseMovementSpeed : ApplyEffect
{
    public override void Apply(PlayerXP player, int value)
    {
        player.movementStats.topSpeed += value;
    }
}

