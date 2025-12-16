using UnityEngine;

[CreateAssetMenu(fileName = "Stat Bonus", menuName = "LevelUp/Stat Bonus")]
public class StatBonus : LevelUpBonus
{
    public float[] valuesPerLevel;
    public ApplyEffect effect;
    private int currentSkillLevel = 0;

    public override void ApplyOrUnlock(PlayerXP playerLevelManager)
    {
        float value = GetValue(currentSkillLevel);
        effect.Apply(playerLevelManager, value);
        currentSkillLevel++;
    }
    public float GetCurrentSkillLevel()
    {
        return currentSkillLevel;
    }
    public float GetMaxSkillLevel()
    {
        return valuesPerLevel.Length;
    }
    // Temporary safety function to avoid out of bounds access
    protected float GetValue(int level)
    {
        if (valuesPerLevel.Length == 0)
        {
            return 0;
        }
        return valuesPerLevel[Mathf.Clamp(level, 0, valuesPerLevel.Length - 1)];
    }

}
