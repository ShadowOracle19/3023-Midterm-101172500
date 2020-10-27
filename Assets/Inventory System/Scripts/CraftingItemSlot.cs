using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using UnityEditorInternal.VersionControl;
using UnityEditor;

public class CraftingItemSlot : MonoBehaviour
{
    // Event callbacks
    public UnityEvent<Item> onItemUse;


    // flag to tell ItemSlot it needs to update itself after being changed
    private bool b_needsUpdate = true;

    // Declared with auto-property
    public Item ItemInSlot { get; private set; }
    public int ItemCount { get; private set; }

    public CursorItem cursorItem;
    public Inventory inventory;
    // scene references
    [SerializeField]
    private TMPro.TextMeshProUGUI itemCountText;

    [SerializeField]
    private Image itemIcon;
    private void Update()
    {
        if (b_needsUpdate)
        {
            UpdateSlot();
        }

        SetContents(inventory.checkRecipe(), 1);
    }

    /// <summary>
    /// Returns true if there is an item in the slot
    /// </summary>
    /// <returns></returns>
    public bool HasItem()
    {
        return ItemInSlot != null;
    }

    /// <summary>
    /// Removes everything in the item slot
    /// </summary>
    /// <returns></returns>
    public void ClearSlot()
    {
        ItemInSlot = null;
        b_needsUpdate = true;
    }

    /// <summary>
    /// Attempts to remove a number of items. Returns number removed
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public int TryRemoveItems(int count)
    {
        if (count > ItemCount)
        {
            int numRemoved = ItemCount;
            ItemCount -= numRemoved;
            b_needsUpdate = true;
            return numRemoved;
        }
        else
        {
            ItemCount -= count;
            b_needsUpdate = true;
            return count;
        }
    }

    /// <summary>
    /// Sets what is contained in this slot
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void SetContents(Item item, int count)
    {
        ItemInSlot = item;
        ItemCount = count;
        b_needsUpdate = true;
    }

    /// <summary>
    /// Activate the item currently held in the slot
    /// </summary>
    public void UseItem()
    {
        
        if (ItemInSlot != null)
        {
            if (ItemCount >= 1)
            {
                cursorItem.SetContents(ItemInSlot, ItemCount);
                ItemInSlot.Use();
                onItemUse.Invoke(ItemInSlot);
                ItemCount--;
                for (int i = 0; i < inventory.craftingSlots.Count; i++)
                {
                    inventory.craftingSlots[i].ItemCount--;
                    inventory.craftingSlots[i].itemCountText.text = inventory.craftingSlots[i].ItemCount.ToString();
                    inventory.craftingSlots[i].b_needsUpdate = true;
                }
                b_needsUpdate = true;
            }
        }
    }
    /// <summary>
    /// Update visuals of slot to match items contained
    /// </summary>
    private void UpdateSlot()
    {
        
        if (ItemCount == 0)
        {
            ItemInSlot = null;
        }

        if (ItemInSlot != null)
        {

            itemCountText.text = ItemCount.ToString();
            itemIcon.sprite = ItemInSlot.Icon;
            itemIcon.gameObject.SetActive(true);
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
        }

        b_needsUpdate = false;
    }
    public void onButtonClick()
    {
        SetContents(cursorItem.ItemInSlot, cursorItem.ItemCount);
    }

}
