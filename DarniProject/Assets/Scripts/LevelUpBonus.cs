using UnityEngine;

[CreateAssetMenu(fileName = "LevelUpBonus", menuName = "LevelUpBonus")]
public class LevelUpBonus : ScriptableObject
{
    public string bonusName;
    public string description;
    public Sprite icon;

    public bool isSpell;
    public float[] levelValues;

    public void ApplyBonus(PlayerXP playerLevelManager, int skillLevel)
    {

    }
    public void UnlockSpell(PlayerXP playerLevelManager)
    {

    }



}
