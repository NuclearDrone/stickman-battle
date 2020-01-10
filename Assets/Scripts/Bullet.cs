using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float damage = 0;
    public string shooter;
    void Start()
    {
        rb.velocity = transform.right * speed;
    }
    void OnTriggerEnter2D(Collider2D hit) {
        if (hit.gameObject.name.Equals(shooter)) {
            return;
        }
        if (hit.gameObject.CompareTag("Player")) {
            PlayerController player = hit.gameObject.GetComponent<PlayerController>();

            player.health -= damage;
        }
        Destroy(gameObject);
    }
}
