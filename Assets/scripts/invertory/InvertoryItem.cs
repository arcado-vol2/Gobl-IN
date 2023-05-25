using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvertoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    public Item item;
    [Header("UI")]
    public Image image;
    public Text count_text;

    [HideInInspector]
    public Transform parent_after_drag;
    [HideInInspector]
    public int count = 1;

    public void Start()
    {
        InitialiseItem(item);
    }

    public void InitialiseItem(Item new_item)
    {
        item = new_item;
        image.sprite = new_item.image;
        UpdateCount();
    }

    public void UpdateCount()
    {
        count_text.text = count.ToString();
        bool text_active = count > 1;
        count_text.gameObject.SetActive(text_active);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parent_after_drag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parent_after_drag);
        image.raycastTarget = true;
    }
}
