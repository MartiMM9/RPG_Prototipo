using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public List<InventoryManager> slots = new List<InventoryManager>();
    public List<ItemManager> items = new List<ItemManager>();

    void Update()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                slots[i].slotImage.sprite = items[i].itemSprite;
                slots[i].slotText.text = items[i].itemName + " x" + items[i].quantity.ToString();
            }
            else
            {
                slots[i].slotImage.sprite = null;
                slots[i].slotText.text = "";
            }
        }
    }
}

[System.Serializable]
public class InventoryManager
{
    public GameObject slot;
    public TextMeshProUGUI slotText;
    public Image slotImage;
}
[System.Serializable]
public class ItemManager
{
    public string itemName;
    public Sprite itemSprite;
    public int quantity;
}
