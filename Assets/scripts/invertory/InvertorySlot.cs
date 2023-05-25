using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvertorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selected_color, original_color;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.color = selected_color;
    }
    public void Deselect()
    {
        image.color = original_color;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
           
            GameObject dropped = eventData.pointerDrag;
            InvertoryItem draggable_item = dropped.GetComponent<InvertoryItem>();
            draggable_item.parent_after_drag = transform;
        }
    }

}
