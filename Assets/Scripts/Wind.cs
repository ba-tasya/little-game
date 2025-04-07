using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float liftForce = 200f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player inside wind zone");
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("rb not null");
                // rb.AddForce(Vector2.up * liftForce, ForceMode2D.Force);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 5f);
            }
        }
    }
}
