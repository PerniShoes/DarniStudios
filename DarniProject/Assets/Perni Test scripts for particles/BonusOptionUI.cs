using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonusOptionUI : MonoBehaviour
{
    public Button buttonRef;
    public Image iconImage;
    public TextMeshProUGUI label;
    private LevelUpBonus assignedBonus;
    private System.Action<LevelUpBonus> onSelect;
    public void Setup(LevelUpBonus bonus, System.Action<LevelUpBonus> onSelectCallback)
    {
        gameObject.SetActive(true);

        assignedBonus = bonus;
        onSelect = onSelectCallback;
        label.text = bonus.bonusName;

        if(iconImage != null && bonus.icon != null)
        {
            iconImage.sprite = bonus.icon;
        }
        buttonRef.onClick.RemoveAllListeners();
        buttonRef.onClick.AddListener(OnClick);
    }
    public void OnClick()
    {
        onSelect?.Invoke(assignedBonus);
    }

}
