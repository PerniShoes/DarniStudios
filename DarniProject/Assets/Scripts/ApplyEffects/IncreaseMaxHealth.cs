using UnityEngine;
public class IncreaseMaxHealth : ApplyEffect
{
    public override void Apply(PlayerXP player, float value)
    {
        player.healthStats.SetMaxHealth((int)value);
    }
}
