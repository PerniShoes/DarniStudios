using UnityEngine;

[CreateAssetMenu(fileName = "ReduceDashCooldown", menuName = "LevelUp/Effects/ReduceDashCooldown")]
public class ReduceDashCooldown : ApplyEffect
{
    public override void Apply(PlayerXP player, float value)
    {
        player.movementStats.dashCooldown -= value;
        // Should add some kind of min dash cd
        if(player.movementStats.dashCooldown <= 0)
        {
            player.movementStats.dashCooldown = 0.1f;
        }

    }
}
