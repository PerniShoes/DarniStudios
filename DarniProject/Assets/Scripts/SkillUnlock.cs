using FullOpaqueVFX;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell Unlock", menuName = "LevelUp/Spell Unlock")]
public class SkillUnlock : LevelUpBonus
{
    public SpellData spellToUnlock;

    public override void ApplyOrUnlock(PlayerXP playerLevelManager, int skillLevel)
    {
        playerLevelManager.UnlockSpell(spellToUnlock);
    }


}
