using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    Consumeable,
    Weapon
}

[CreateAssetMenu(menuName = "item/Create new Item", fileName = "item", order = 0)]
public class Item : ScriptableObject
{
    public string name;
    public int price;
    public Sprite sprite;
    public ItemType type;
    public int damage;
    public int ammunition;
    public int fireRate;
}