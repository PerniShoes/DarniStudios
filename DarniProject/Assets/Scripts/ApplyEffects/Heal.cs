using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "LevelUp/Effects/Heal")]
public class Heal : ApplyEffect
{
    public override void Apply(PlayerXP player, float value)
    {
        player.healthStats.Heal((int)value);
    }
}
