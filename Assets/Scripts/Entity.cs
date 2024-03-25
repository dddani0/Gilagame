using UnityEngine;

[CreateAssetMenu(menuName = "Entity", fileName = "new Entity")]
public class Entity : ScriptableObject
{
    public new string name;
    public int health;
    public float speed;
    public float distance;
    public const double VisionConeDegree = 70D;
}