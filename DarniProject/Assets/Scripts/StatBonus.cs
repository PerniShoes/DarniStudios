using UnityEngine;

[CreateAssetMenu(fileName = "Stat Bonus", menuName = "LevelUp/Stat Bonus")]
public class StatBonus : LevelUpBonus
{
    public int[] valuesPerLevel;
    public ApplyEffect effect;

    public override void ApplyOrUnlock(PlayerXP playerLevelManager, int skillLevel)
    {
        int value = GetValue(skillLevel);
        effect.Apply(playerLevelManager, value);
    }

    // Temporary safety function to avoid out of bounds access
    protected int GetValue(int level)
    {
        return valuesPerLevel[Mathf.Clamp(level, 0, valuesPerLevel.Length - 1)];
    }

}
