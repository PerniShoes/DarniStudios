using UnityEngine;

public abstract class LevelUpBonus : ScriptableObject
{
    public string bonusName;
    public string description;
    public Sprite icon;


    public abstract void ApplyOrUnlock(PlayerXP playerLevelManager);
 


}
