using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private List<ItemSlot> Slots = new();

    public void SetSpriteToSlot(int index, Sprite newSprite)
    {
        if (index >= Slots.Count)
        {
            return;
        }
        
        Slots[index].SetSprite(newSprite);
    }

    public int SlotCount => Slots.Count;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Slots.Add(transform.GetChild(i).GetComponent<ItemSlot>());
        }
    }
}
