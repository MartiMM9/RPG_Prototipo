using UnityEngine;

public class RayController : MonoBehaviour
{
    [SerializeField] 
    private float speed;
    [SerializeField] 
    private float damage;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject);
        }    
    }
}
