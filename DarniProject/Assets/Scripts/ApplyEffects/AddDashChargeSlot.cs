using UnityEngine;
public class AddDashChargeSlot : ApplyEffect
{
    public override void Apply(PlayerXP player, int value)
    {
        player.movementStats.AddDashChargeSlots(value);
    }
}
