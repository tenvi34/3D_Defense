using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Image slotImage;
    
    public void SetSprite(Sprite newSprite)
    {
        slotImage.sprite = newSprite;
        if (newSprite != null)
            slotImage.color = Color.white;
    }
    
    void Start()
    {
        slotImage = GetComponent<Image>();
    }
}
