using FullOpaqueVFX;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CoolddownTracker : MonoBehaviour
{

    [Header("SpellManagers")]
    public VFX_SpellManager First;
    public VFX_SpellManager Second;
    public VFX_SpellManager Third;
    public VFX_SpellManager Fourth;
    public VFX_SpellManager Fifth;
    public VFX_SpellManager Sixth;

    private float[] spellCooldowns;
    private float[] currentSpellCooldowns;
    private List<GameObject> imageSpellSlots;

    void Start()
    {

        FindAndSetSpellSlots();

        currentSpellCooldowns = Enumerable.Repeat(0.0f, 6).ToArray();
        spellCooldowns = new float[6];

        if (First != null && First.currentSpell != null)
            spellCooldowns[0] = First.currentSpell.cooldown;

        if (Second != null && Second.currentSpell != null)
            spellCooldowns[1] = Second.currentSpell.cooldown;

        if (Third != null && Third.currentSpell != null)
            spellCooldowns[2] = Third.currentSpell.cooldown;

        if (Fourth != null && Fourth.currentSpell != null)
            spellCooldowns[3] = Fourth.currentSpell.cooldown;

        if (Fifth != null && Fifth.currentSpell != null)
            spellCooldowns[4] = Fifth.currentSpell.cooldown;

        if (Sixth != null && Sixth.currentSpell != null)
            spellCooldowns[5] = Sixth.currentSpell.cooldown;

    }
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i< currentSpellCooldowns.Length; ++i)
        {
            currentSpellCooldowns[i] = Mathf.Max(0f, currentSpellCooldowns[i] - Time.deltaTime);
        }
        UpdateImages();
    }

    void UpdateImages()
    {
        for(int i = 0; i< currentSpellCooldowns.Length; ++i)
        {
            float fillAmountCD = (currentSpellCooldowns[i]/ spellCooldowns[i]);
            Transform child = imageSpellSlots[i].transform.Find("Cooldown Layer");

            child.GetComponent<Image>().fillAmount = fillAmountCD;

        }
    }

    public void StartCooldown(int spellSlotId)
    {
        spellSlotId -= 1;
        currentSpellCooldowns[spellSlotId] = spellCooldowns[spellSlotId];
    }

    void UpdateCooldown(float newCooldown, int spellSlotId)
    {
        if(spellSlotId > 0 && spellSlotId < spellCooldowns.Length)
        {
            spellCooldowns[spellSlotId] = newCooldown;
        }
    }

    void FindAndSetSpellSlots()
    {
        GameObject parent = GameObject.Find("SpellCDTracker");

        if (parent == null)
        {
            return;
        }
        int childrenCount = parent.transform.childCount;
        imageSpellSlots = new List<GameObject>(childrenCount);

        for (int i = 0; i < childrenCount; i++)
        { 
            imageSpellSlots.Add(null);
            imageSpellSlots[i] = parent.transform.GetChild(i).gameObject;
        }
    }


}
