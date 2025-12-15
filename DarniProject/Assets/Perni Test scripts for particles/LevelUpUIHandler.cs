using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    public static LevelUpUI Instance;

    public GameObject bonusButtonPrefab;
    public Transform optionsContainer;

    private PlayerXP player;
    private int skillLevel;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowOptions(PlayerXP playerXP, LevelUpBonus[] options)
    {
        player = playerXP;

        foreach (Transform child in optionsContainer)
            Destroy(child.gameObject);

        foreach (var bonus in options)
        {
            var buttonObj = Instantiate(bonusButtonPrefab, optionsContainer);
            var optionUI = buttonObj.GetComponent<BonusOptionUI>();
            optionUI.Setup(bonus, OnBonusSelected);
        }

        gameObject.SetActive(true);
    }

    private void OnBonusSelected(LevelUpBonus bonus)
    {
        bonus.ApplyOrUnlock(player);

        gameObject.SetActive(false);
        StateManager.Instance.UnpauseGame();
    }
}
