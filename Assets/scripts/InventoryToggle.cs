using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    public GameObject Inventory;

    public void OnOpenButtonClick()
    {
        Inventory.SetActive(true);
    }
    public void OnCloseButtonClick()
    {
        Inventory.SetActive(false);
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!Inventory.activeSelf)
                OnOpenButtonClick();

            else
                OnCloseButtonClick();
        }
    }
}
