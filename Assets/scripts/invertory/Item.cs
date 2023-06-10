using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
   [Header("Only gameplay")]
    public ItemType type;
    public ActionType action_type;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;
    
    [Header("Both")]
    public Sprite image;
}

public enum ItemType
{
    bomb,
    mine,
    detonator,
    weapon,
    key,
}

public enum ActionType
{
    Mele,
    Range,
}