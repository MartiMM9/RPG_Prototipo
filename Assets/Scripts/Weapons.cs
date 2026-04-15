using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon_SO")]
public class Weapons : ScriptableObject
{
    public string weaponName;
    public int handSlots;
    public float damage;
    public float weight;
    public float hitDistance;
    public GameObject prefabMesh;
    public float range;
}
