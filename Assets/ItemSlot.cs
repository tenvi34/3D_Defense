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
    
    // Start is called before the first frame update
    void Start()
    {
        slotImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
