using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashChargesSetup : MonoBehaviour
{
    public Movement playerMovement;
    public GameObject chargeSlotTemplate;
    public float chargeTemplateScale;
    public float horizontalOffset;
    // private float slotHeight; // Not used
    private float slotWidth;
    private int amountOfSlots = 0;

    void Start()
    {
        RectTransform slotTransform = chargeSlotTemplate.GetComponent<RectTransform>();
        // slotHeight = 100f * chargeTemplateScale;
        slotWidth = 100f * chargeTemplateScale;

    }
    public void SetSlotAmount(int amount)
    {
        if (amount < 0) amount = 0;

        // Clear pool for proper setup
        for (int i = 0; i < playerMovement.dashCharges.Count; i++)
        {
            ObjectPoolManager.ReturnObjectToPool(playerMovement.dashCharges[i].gameObject.transform.parent.transform.parent.gameObject);
        }
        amountOfSlots = amount;

        float spacing = slotWidth + horizontalOffset;
        float centerIndex = (amount - 1) / 2f;

        playerMovement.dashCharges.Clear();
        int currentCharges = 0;

        for (int i = 0; i < amountOfSlots; i++)
        {
            float offsetFromCenter = (i - centerIndex) * spacing;

            Vector2 spawnPos = new Vector2(
                chargeSlotTemplate.transform.position.x + offsetFromCenter,
                chargeSlotTemplate.transform.position.y);

            GameObject chargeTemplateClone = ObjectPoolManager.SpawnObject(
                chargeSlotTemplate,
                spawnPos,
                Quaternion.identity,
                ObjectPoolManager.PoolType.DashUI
                );

            chargeTemplateClone.transform.localScale = Vector3.one * chargeTemplateScale;
            chargeTemplateClone.SetActive(true);
            // Inefficient but happens rarely
            playerMovement.SetDashImageReference(
                chargeTemplateClone.transform.Find("Rect Mask").transform.Find("Fill").GetComponentInChildren<Image>()
                );
            currentCharges++;
        }
        playerMovement.SetTotalDashCharges(currentCharges);

    }
    public int GetTotalChargeSlots()
    {
        return amountOfSlots;
    }
    public void AddDashChargeSlots(int amount)
    {
        SetSlotAmount(amountOfSlots+amount);
    }
    public void RemoveDashChargeSlots(int amount)
    {
        SetSlotAmount(amountOfSlots-amount);
    }


}
