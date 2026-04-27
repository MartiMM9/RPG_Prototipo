using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField]
    private string type;
    [SerializeField]
    private float quantity;
    [SerializeField]
    private PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Consume()
    {
        player.GainStat(type,quantity);
    }
}
