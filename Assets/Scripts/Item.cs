using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "item/Create new Item", fileName = "item", order = 0)]
public class Item : ScriptableObject
{
    public string name;
    public int price;
    public bool available;
    public Sprite sprite;
}