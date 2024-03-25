using UnityEngine;

[CreateAssetMenu(menuName = "Gun", fileName = "new Gun")]
public class Gun : ScriptableObject
{
    public new string name;
    public int ammunition;
    public int damage;
    public int fireRate;
}