using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonusOptionUI : MonoBehaviour
{
    public TextMeshProUGUI label;
    private LevelUpBonus assignedBonus;
    private System.Action<LevelUpBonus> onSelect;
    public void Setup(LevelUpBonus bonus, System.Action<LevelUpBonus> onSelectCallback)
    {
        assignedBonus = bonus;
        onSelect = onSelectCallback;
        label.text = bonus.bonusName;
    }
    public void OnClick()
    {
        onSelect?.Invoke(assignedBonus);
    }

}
