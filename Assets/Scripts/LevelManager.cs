using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private bool inventorySwitch;
    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject weaponsPanel;
    public List<InventoryManager> slots = new List<InventoryManager>();
    public List<ItemManager> items = new List<ItemManager>();

    void Update()
    {
        if (!inventorySwitch)
        {
            inventoryPanel.SetActive(true);
            weaponsPanel.SetActive(false);
            for (int i = 0; i < slots.Count; i++)
            {
                if (i < items.Count)
                {
                    slots[i].slot.SetActive(true);
                    slots[i].slotImage.sprite = items[i].itemSprite;
                    slots[i].slotText.text = items[i].itemName + " x" + items[i].quantity.ToString();
                }
                else
                {
                    slots[i].slotImage.sprite = null;
                    slots[i].slotText.text = "";
                    slots[i].slot.SetActive(false);
                }
            }
        }
        else
        {
            inventoryPanel.SetActive(false);
            weaponsPanel.SetActive(true);
        }
    }
    
    public void toggleMenus(bool thing)
    {
        inventorySwitch = thing;
    }

    public void Consume(int i)
    {
        if (items[i].quantity > 0)
        {
            player.GainStat(items[i].thisPotion.type, items[i].thisPotion.quantity);
            items[i].quantity--;
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
    public Potion thisPotion;
}
