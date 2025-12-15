using UnityEngine;

[CreateAssetMenu(fileName = "ReduceDashCooldown", menuName = "LevelUp/Effects/ReduceDashCooldown")]
public class ReduceDashCooldown : ApplyEffect
{
    public override void Apply(PlayerXP player, float value)
    {
        player.movementStats.dashCooldown -= value;
    }
}
