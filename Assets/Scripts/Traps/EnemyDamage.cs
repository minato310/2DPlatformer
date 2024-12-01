using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage;// Lượng sát thương

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")// Nếu va chạm là người chơi
            collision.GetComponent<Health>().TakeDamage(damage);// Gây sát thương
    }
}