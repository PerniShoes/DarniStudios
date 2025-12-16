using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseMaxHealth", menuName = "LevelUp/Effects/IncreaseMaxHealth")]
public class IncreaseMaxHealth : ApplyEffect
{
    public override void Apply(PlayerXP player, float value)
    {
        player.healthStats.maxHealth += (int)value;
        player.healthStats.SetMaxHealth(player.healthStats.maxHealth);

        player.healthStats.currentHealth += (int)value;
        player.healthStats.SetHealth(player.healthStats.currentHealth);
    }
}
