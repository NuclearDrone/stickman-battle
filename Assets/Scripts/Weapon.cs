using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject {

    public new string name;
    public float damage;
    public float attackCooldown;
    public Sprite sprite;

}
