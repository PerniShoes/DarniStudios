using UnityEngine;
public class AddDashChargeSlot : ApplyEffect
{
    public override void Apply(PlayerXP player, float value)
    {
        player.movementStats.AddDashChargeSlots((int)value);
    }
}
