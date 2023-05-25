using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertiryManager : MonoBehaviour
{
    [SerializeField]
    private int max_stack = 4;
    public InvertorySlot[] invertory_slots;
    public GameObject invertiry_item_prefab;

    [SerializeField]
    private int max_slots = 3;
    private int selected_slot = 0;

    private void Start()
    {
        invertory_slots[selected_slot].Select();
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool is_num = int.TryParse(Input.inputString, out int number);
            if (is_num && number > 0 && number < 4) {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    public void ChangeSelectedSlot(int new_value){
        invertory_slots[selected_slot].Deselect();
        invertory_slots[new_value].Select();
        selected_slot = new_value;
    }
    
    public bool AddItem(Item item)
    {
        for (int i = 0; i < invertory_slots.Length; i++)
        {
            InvertorySlot slot = invertory_slots[i];
            InvertoryItem item_in_slot = slot.GetComponentInChildren<InvertoryItem>();
            if (item_in_slot != null &&
                item_in_slot.item == item &&
                item_in_slot.count < max_stack &&
                item_in_slot.item.stackable == true)
            {
                item_in_slot.count++;
                item_in_slot.UpdateCount();
                return true;
            }
        }

        for (int i = 0; i< invertory_slots.Length; i++)
        {
            InvertorySlot slot = invertory_slots[i];
            InvertoryItem item_in_slot = slot.GetComponentInChildren<InvertoryItem>();
            if (item_in_slot == null)
            {
                SpawnItem(item, slot);
                return true;
            }
        }
        return false;
    }

    void SpawnItem(Item item, InvertorySlot slot)
    {
        GameObject new_item_GO = Instantiate(invertiry_item_prefab, slot.transform);
        InvertoryItem invertory_item = new_item_GO.GetComponentInChildren<InvertoryItem>();
        invertory_item.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InvertorySlot slot = invertory_slots[selected_slot];
        InvertoryItem item_in_slot = slot.GetComponentInChildren<InvertoryItem>();
        if (item_in_slot != null)
        {
            if (use)
            {
                item_in_slot.count--;
                if (item_in_slot.count <= 0)
                {
                    Destroy(item_in_slot.gameObject);
                }
                else
                {
                    item_in_slot.UpdateCount();
                }
            }
            return item_in_slot.item;
        }
        return null;
    }

    public void NextSlot()
    {
        int tmp = selected_slot + 1;
        if (tmp >= max_slots)
        {
            tmp = 0;
        }
        ChangeSelectedSlot(tmp);
    }

    public void PrevSlot()
    {
        int tmp = selected_slot - 1;
        if (tmp < 0)
        {
            tmp = max_slots-1;
        }
        ChangeSelectedSlot(tmp);
    }
}
