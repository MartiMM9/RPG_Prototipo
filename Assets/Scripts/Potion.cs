using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField]
    private string type;
    [SerializeField]
    private float quantity;
    [SerializeField]
    private CharacterController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    private void Consume()
    {
        player.GainStat(type,quantity);
    }
}
