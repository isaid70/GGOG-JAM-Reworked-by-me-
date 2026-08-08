using UnityEngine;

public class GiveDamageToPlayer : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.GetDamage(damage);
        }
    }
}

