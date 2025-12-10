using UnityEngine;
public class ReduceDashCooldown : ApplyEffect
{
    public override void Apply(PlayerXP player, float value)
    {
        player.movementStats.dashCooldown -= value;
    }
}
