using UnityEngine;

public class DashChargesSetup : MonoBehaviour
{
    public GameObject chargeSlotTemplate;
    public float horizontalOffset;
    private float slotHeight;
    private float slotWidth;
    private int amountOfSlots = 0;

    void Start()
    {
        RectTransform slotTransform = chargeSlotTemplate.GetComponent<RectTransform>();
        slotHeight = slotTransform.rect.height;
        slotWidth = slotTransform.rect.width;

    }
    public void SetSlotAmount(int amount)
    {
        if (amount < 0) amount = 0;

        // Clear pool for proper setup
        for (int i = 0; i < amountOfSlots; i++)
        {
            ObjectPoolManager.ReturnObjectToPool(chargeSlotTemplate);
        }
        amountOfSlots = amount;

        float spacing = slotWidth + horizontalOffset;
        float centerIndex = (amount - 1) / 2f;

        for (int i = 0; i < amountOfSlots; i++)
        {
            float offsetFromCenter = (i - centerIndex) * spacing;

            Vector2 spawnPos = new Vector2(
                chargeSlotTemplate.transform.position.x + offsetFromCenter,
                chargeSlotTemplate.transform.position.y);

            ObjectPoolManager.SpawnObject(
                chargeSlotTemplate,
                spawnPos,
                Quaternion.identity,
                ObjectPoolManager.PoolType.UI
                );
        }

    }

    public void AddDashChargeSlots(int amount)
    {
        amountOfSlots += amount;
        SetSlotAmount(amountOfSlots);
    }
    public void RemoveDashChargeSlots(int amount)
    {
        amountOfSlots -= amount;
        SetSlotAmount(amountOfSlots);
    }


}
