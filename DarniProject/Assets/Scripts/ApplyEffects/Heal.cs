using UnityEngine;
public class Heal : ApplyEffect
{
    public override void Apply(PlayerXP player, int value)
    {
        player.healthStats.Heal(value);
    }
}
