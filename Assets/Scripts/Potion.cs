using UnityEngine;

public class Potion : MonoBehaviour
{
    public string type;
    public float quantity;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private LevelManager lm;
    [SerializeField]
    private Sprite sprite;

    private void Consume()
    {
        player.GainStat(type,quantity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lm.AddItem(type, sprite, 1, this);
            Destroy(gameObject);
        }
    }
}
