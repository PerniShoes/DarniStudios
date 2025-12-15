using UnityEngine;

[CreateAssetMenu(fileName = "DashChargeSlot", menuName = "LevelUp/Effects/DashChargeSlot")]
public class DashChargeSlot : ApplyEffect
{
    public override void Apply(PlayerXP player, float value)
    {
        player.movementStats.AddDashChargeSlots((int)value);
    }
}
