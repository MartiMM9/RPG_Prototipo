using UnityEngine;

public class Potion : MonoBehaviour
{
    public string type;
    public float quantity;
    [SerializeField]
    private PlayerController player;

    private void Consume()
    {
        player.GainStat(type,quantity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Consume();
            Destroy(gameObject);
        }
    }
}
