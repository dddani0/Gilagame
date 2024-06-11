using UnityEngine;

[CreateAssetMenu(menuName = "Gun", fileName = "new Gun")]
public class Gun : ScriptableObject
{
    public new string name;
    public int ammunition;
    public int damage;
    public int fireRate;


    private Gun(string name, int ammunition, int damage, int fireRate)
    {
        this.name = name;
        this.ammunition = ammunition;
        this.damage = damage;
        this.fireRate = fireRate;
    }

    public static Gun CreateGun(string _name, int _ammunition, int _dmg, int _fireRate) =>
        new Gun(_name, _ammunition, _dmg, _fireRate);
}