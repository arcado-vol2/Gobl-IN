using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_invertory : MonoBehaviour
{
    public List<Item> items;
    public InvertiryManager manager;

    public void Add_item(Item item)
    {
        if (GetComponent<contoller_player>().user_control)
        {
            items.Add(item);
            manager.AddItem(item);
        }
        else
        {
            items.Add(item);
        }
    }

}
