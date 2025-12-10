using UnityEngine;
public class IncreaseMaxHealth : ApplyEffect
{
    public override void Apply(PlayerXP player, int value)
    {
        player.healthStats.SetMaxHealth(value);
    }
}
